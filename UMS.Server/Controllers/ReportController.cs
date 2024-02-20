using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UMS.Core.Dtos;
using SelectPdf;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using UMS.Core.Interfaces;
using Microsoft.AspNetCore.Routing.Template;
using RazorEngineCore;
using UMS.Core.Utils;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using HtmlToOpenXml;

namespace UMS.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReportController : ControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IStudentService _studentService;
		private readonly ISubjectService _subjectService;
		private readonly ITeacherService _teacherService;

		public ReportController(
			IWebHostEnvironment webHostEnvironment,
			IStudentService studentService,
			ISubjectService subjectService,
			ITeacherService teacherService)
		{
			_webHostEnvironment = webHostEnvironment;
			_studentService = studentService;
			_subjectService = subjectService;
			_teacherService = teacherService;
		}
		[HttpGet("Summary")]
		[Authorize(Roles = UserRoles.Admin)]
		public IActionResult GetSummaryPdf()
		{
			var studentResponse = _studentService.GetAll();
			var subjectResponse = _subjectService.GetAll();
			var teacherResponse = _teacherService.GetAll();

			if (studentResponse.Data == null || studentResponse.StatusCode != HttpStatusCode.OK ||
				subjectResponse.Data == null || subjectResponse.StatusCode != HttpStatusCode.OK ||
				teacherResponse.Data == null || teacherResponse.StatusCode != HttpStatusCode.OK)
			{
				return StatusCode(500, "Failed to get data.");
			}

			string templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources/Templates", "Summary.html");
			string htmlContent = System.IO.File.ReadAllText(templatePath);
			var deletedStudentsCount = studentResponse.Data.Where(x => x.IsDeleted).Count();
			var deletedSubjectsCount = subjectResponse.Data.Where(x => x.IsDeleted).Count();
			var deletedTeachersCount = teacherResponse.Data.Where(x => x.IsDeleted).Count();
			var data = new
			{
				StudentsCount = studentResponse.Data.Count,
				DeletedStudentsCount = deletedStudentsCount,
				SubjectsCount = subjectResponse.Data.Count,
				DeletedSubjectsCount = deletedSubjectsCount,
				TeachersCount = teacherResponse.Data.Count,
				DeletedTeachersCount = deletedTeachersCount
			};

			var engine = new RazorEngine().Compile(htmlContent);

			var renderedHtml = engine.Run(data);

			HtmlToPdf converter = new HtmlToPdf();
			PdfDocument doc = converter.ConvertHtmlString(renderedHtml);
			byte[] pdfBytes = doc.Save();
			doc.Close();

			return File(
				pdfBytes, 
				"application/pdf",
				"Summary.pdf");

		}

		[HttpGet("Tables")]
		[Authorize(Roles = UserRoles.Admin)]
		public IActionResult GetTablesWord()
		{
			var studentResponse = _studentService.GetAll();
			var subjectResponse = _subjectService.GetAll();
			var teacherResponse = _teacherService.GetAll();

			if (studentResponse.Data == null || studentResponse.StatusCode != HttpStatusCode.OK ||
				subjectResponse.Data == null || subjectResponse.StatusCode != HttpStatusCode.OK ||
				teacherResponse.Data == null || teacherResponse.StatusCode != HttpStatusCode.OK)
			{
				return StatusCode(500, "Failed to get data.");
			}

			string templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources/Templates", "Tables.html");
			string htmlContent = System.IO.File.ReadAllText(templatePath);
			var data = new
			{
				Students = studentResponse.Data.OrderBy(x => x.IsDeleted),
				Subjects = subjectResponse.Data.OrderBy(x => x.IsDeleted),
				Teachers = teacherResponse.Data.OrderBy(x => x.IsDeleted),
			};

			var engine = new RazorEngine().Compile(htmlContent);

			var renderedHtml = engine.Run(data);

			using MemoryStream stream = new MemoryStream();
			using (WordprocessingDocument package = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
			{
				MainDocumentPart mainPart = package.AddMainDocumentPart();
				mainPart.Document = new Document();
				Body body = mainPart.Document.AppendChild(new Body());

				HtmlConverter converter = new HtmlConverter(mainPart);
				converter.ParseHtml(renderedHtml);
				mainPart.Document.Save();
			}

			stream.Position = 0;

			return File(
				stream.ToArray(), 
				"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				"Tables.doc");

		}

	}
}
