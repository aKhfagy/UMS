using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Utils;
using UMS.Core.Dtos;
using UMS.Infrastructure.Repository;
using UMS.Core.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace UMS.Application.Services
{
	public class ServiceBase<TModel, TDto> : IServiceBase<TModel, TDto> where TModel : class where TDto : class
	{
		protected readonly IRepository<TModel> _repository;
		protected readonly Mapper _mapper;
		public ServiceBase(IRepository<TModel> repository) 
		{
			_repository = repository;
			_mapper = MapperConfig.InitializeAutomapper();
		}

		public virtual ResultDto<TDto> Add(TDto obj)
		{
			try
			{
				var model = _mapper.Map<TModel>(obj);
				_repository.Insert(model);
				_repository.Save();
				return new ResultDto<TDto>()
				{
					Data = _mapper.Map<TDto>(model),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<TDto>()
				{
					Message = $"Error: ${ex.Message}",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}

		}

		public virtual ResultDto Delete(int id)
		{
			try
			{
				_repository.Delete(id);
				_repository.Save();
				return new ResultDto()
				{
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch(Exception ex)
			{
				return new ResultDto()
				{
					Message = $"Error: ${ex.Message}",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public virtual ResultDto<TDto> Edit(TDto obj)
		{
			try
			{
				var model = _mapper.Map<TModel>(obj);
				_repository.Update(model);
				_repository.Save();
				return new ResultDto<TDto>()
				{
					Data = _mapper.Map<TDto>(model),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<TDto>()
				{
					Message = $"Error: ${ex.Message}",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public virtual ResultDto<List<TDto>> GetAll()
		{
			try
			{
				var list = _repository.GetAll().ToList();
				return new ResultDto<List<TDto>>()
				{
					Data = _mapper.Map<List<TModel>, List<TDto>>(list),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<List<TDto>>()
				{
					Data = null,
					Message = $"Error: ${ex.Message}",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public virtual ResultDto<TDto> GetById(int id)
		{
			try
			{
				var model = _repository.GetById(id);
				return new ResultDto<TDto>()
				{
					Data = _mapper.Map<TDto>(model),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<TDto>()
				{
					Data = null,
					Message = $"Error: ${ex.Message}",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
