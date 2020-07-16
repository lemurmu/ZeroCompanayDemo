using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Model
{
    /// <summary>
    /// 学生信息
    /// </summary>
    public class StudentInfo
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 学生名字
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNumber { get; set; }

    }
}
