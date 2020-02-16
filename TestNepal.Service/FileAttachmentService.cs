using TestNepal.Dtos;
using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;
using TestNepal.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Expressions;

namespace TestNepal.Service
{
    public class FileAttachmentService : IFileAttachmentService
    {
        private IFileAttachmentRepository _languageRepository;
        IUnitOfWork _unitOfWork;
        public FileAttachmentService(IFileAttachmentRepository languageRepository, IUnitOfWork unitOfWork)
        {
            _languageRepository = languageRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<FileAttachment> GetAll()
        {
            return _languageRepository.GetAll();
        }
        public IEnumerable<FileAttachment> GetAll(Expression<Func<FileAttachment, bool>> where = null)
        {
            return _languageRepository.GetMany(where).ToList();
        }
        public IEnumerable<FileAttachment> GetAll(params Expression<Func<FileAttachment, object>>[] includeExpressions)
        {
            return _languageRepository.GetAll(includeExpressions);
        }
        public IEnumerable<FileAttachment> GetAll(Expression<Func<FileAttachment, bool>> where = null, params Expression<Func<FileAttachment, object>>[] includeExpressions)
        {
            return _languageRepository.GetMany(where, includeExpressions);
        }
        public void Create(FileAttachment model)
        {
            _languageRepository.Add(model);
        }
        public void Update(FileAttachment model)
        {
            _languageRepository.Update(model);
        }
        public FileAttachment GetFileAttachmentById(int Id)
        {
            FileAttachment model;
            if (Id == 0)
            {
                model = new Entities.FileAttachment();
            }
            else
            {
                model = _languageRepository.GetById(Id);
            }
            return model;
        }
        public FileAttachment GetFileAttachmentById(int Id, Expression<Func<FileAttachment, bool>> where = null, params Expression<Func<FileAttachment, object>>[] includeExpressions)
        {
            return _languageRepository.GetById(Id, where, includeExpressions);
        }

        public void Delete(FileAttachment model)
        {
            _languageRepository.Delete(model);
        }
        public void SaveChanges()
        {
            _languageRepository.SaveChanges();
        }
    }
}
