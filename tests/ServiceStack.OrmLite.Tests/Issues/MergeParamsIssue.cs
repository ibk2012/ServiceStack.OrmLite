﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack.OrmLite.Tests.Shared;
using ServiceStack.Text;

namespace ServiceStack.OrmLite.Tests.Issues
{
    public class Record
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    [TestFixture]
    public class MergeParamsIssue : OrmLiteTestBase
    {
        [Test]
        public void Does_merge_params_correctly()
        {
            using (var db = OpenDbConnection())
            {
                db.DropAndCreateTable<Record>();

                db.InsertAll(new[] {
                    new Record { Id = 1, Value = 1, },
                });

                var x1 = db.SingleById<Record>(1);
                var q = db.From<Record>();
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);
                q.Where(x => x.Value == 1);

                var qIn = db.From<Record>();
                qIn.Where(x => x.Value == 1);
                qIn.Where(x => x.Value == 1 && x.Value == 1);
                q.Where(x => Sql.In(x.Value, qIn));
                var expression = q.ToSelectStatement();
                expression.Print();

                for (var i = 0; i < 11; i++)
                {
                    Assert.That(expression, Does.Contain(db.GetDialectProvider().ParamString + i));
                }
            }
        }
    }
}
