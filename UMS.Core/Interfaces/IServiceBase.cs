using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;

namespace UMS.Core.Interfaces
{
	public interface IServiceBase<TModel, TDto> where TModel : class where TDto : class
	{
		public ResultDto<List<TDto>> GetAll();
		public ResultDto<TDto> GetById(int id);
		public ResultDto<TDto> Edit(TDto obj);
		public ResultDto Delete(int id);
		public ResultDto<TDto> Add(TDto obj);
	}
}
