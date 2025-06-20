﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.MongoDb
{
    public class MongoDbQueryable<T> : QueryableProvider<T>
    {
        public override int Count()
        { 
            return  GetCount(); ;
        }
        public override Task<int> CountAsync()
        {
            return GetCountAsync();
        }
        public override ISugarQueryable<T> With(string withString)
        {
            return this;
        }

        public override ISugarQueryable<T> PartitionBy(string groupFileds)
        {
            this.GroupBy(groupFileds);
            return this;
        }
    }
    public class MongoDbQueryable<T, T2> : QueryableProvider<T, T2>
    {
        public new ISugarQueryable<T, T2> With(string withString)
        {
            return this;
        }
    }
    public class MongoDbQueryable<T, T2, T3> : QueryableProvider<T, T2, T3>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4> : QueryableProvider<T, T2, T3, T4>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5> : QueryableProvider<T, T2, T3, T4, T5>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6> : QueryableProvider<T, T2, T3, T4, T5, T6>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7> : QueryableProvider<T, T2, T3, T4, T5, T6, T7>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7, T8> : QueryableProvider<T, T2, T3, T4, T5, T6, T7, T8>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> : QueryableProvider<T, T2, T3, T4, T5, T6, T7, T8, T9>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : QueryableProvider<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : QueryableProvider<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {

    }
    public class MongoDbQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : QueryableProvider<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {

    }
}
