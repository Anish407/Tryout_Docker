using CalllerApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallerApi.Infra.Handler
{
    public class StudentHandler(IRepository repository) : IStudentHandler
    {
        public string GetStudentName(int studentId)
        {
            return repository.GetData();
        }
    }
}
