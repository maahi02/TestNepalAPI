using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace TestNepal.Api.Helpers
{
    public static class CommonHelper
    {

        public static List<KeyValue> GetModelStateError(ModelStateDictionary data)
        {
            var errors = new List<KeyValue>();
            foreach (var state in data)
            {
                var state1 = state;
                var keyval = state.Value.Errors.Select(error => new KeyValue { Key = state1.Key, Value = error.ErrorMessage });
                var keyValues = keyval as KeyValue[] ?? keyval.ToArray();
                foreach (var item in keyValues.Where(item => string.IsNullOrEmpty(item.Value)))
                {
                    var key = item.Key.Replace("model.", "");
                    item.Value = "Invalid " + key;
                }
                errors.AddRange(keyValues);
                //errors.AddRange(state.Value.Errors.Select(error => new KeyValue { Key = state.Key, Value = error.ErrorMessage }));
            }
            return errors;
        }

        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }

    }
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }



}


public abstract class PagedResultBase
{
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }

    public int FirstRowOnPage
    {

        get { return (CurrentPage - 1) * PageSize + 1; }
    }

    public int LastRowOnPage
    {
        get { return Math.Min(CurrentPage * PageSize, RowCount); }
    }
}

public class PagedResult<T> : PagedResultBase where T : class
{
    public IList<T> Results { get; set; }

    public PagedResult()
    {
        Results = new List<T>();
    }
}
