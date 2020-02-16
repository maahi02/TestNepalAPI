using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Entities;
using System.Linq.Expressions;
using TestNepal.Dtos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestNepal.Service.Infrastructure
{
    public interface IFileAttachmentService
    {
        IEnumerable<FileAttachment> GetAll();
        IEnumerable<FileAttachment> GetAll(Expression<Func<FileAttachment, bool>> where = null);
        IEnumerable<FileAttachment> GetAll(params Expression<Func<FileAttachment, object>>[] includeExpressions);
        IEnumerable<FileAttachment> GetAll(Expression<Func<FileAttachment, bool>> where = null, params Expression<Func<FileAttachment, object>>[] includeExpressions);
        void Create(FileAttachment model);
        void Update(FileAttachment model);
        FileAttachment GetFileAttachmentById(int Id);
        FileAttachment GetFileAttachmentById(int Id, Expression<Func<FileAttachment, bool>> where = null, params Expression<Func<FileAttachment, object>>[] includeExpressions);
        void Delete(FileAttachment model);
        void SaveChanges();
    }
}
