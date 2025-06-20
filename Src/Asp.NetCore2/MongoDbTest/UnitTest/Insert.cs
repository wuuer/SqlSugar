﻿using SqlSugar.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbTest 
{
    public class Insert
    {
        internal static void Init()
        {
            var db = DBHelper.DbHelper.GetNewDb();
            db.CodeFirst.InitTables<Student>();
            db.DbMaintenance.TruncateTable<Student>();

            db.Insertable(new Student() { Age = 1, Name = "11", SchoolId = "111", CreateDateTime = DateTime.Now }).ExecuteCommand();
            db.Insertable(new List<Student>() {
            new Student() { Age = 2, Name = "22", SchoolId = "222", CreateDateTime = DateTime.Now },
            new Student() { Age = 3, Name = "33", SchoolId = "333", CreateDateTime = DateTime.Now }
            }).ExecuteCommand();

            if (db.Queryable<Student>().Count() != 3) Cases.ThrowUnitError();
            var list=db.Queryable<Student>().ToList();
            if (list.First().Name!="11"|| list.Last().Name != "33") Cases.ThrowUnitError();
        }
        [SqlSugar.SugarTable("UnitStudent1ddsfhssds3z1")]
        public class Student : MongoDbBase
        {
            public string Name { get; set; }

            public string SchoolId { get; set; }

            public int Age { get; set; }

            public DateTime CreateDateTime { get; set; }
        }
    }
}
