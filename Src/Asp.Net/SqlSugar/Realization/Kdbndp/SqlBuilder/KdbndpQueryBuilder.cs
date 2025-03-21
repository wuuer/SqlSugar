﻿using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlSugar
{
    public partial class KdbndpQueryBuilder : QueryBuilder
    {
        #region Sql Template
        public override string SqlTemplate 
        {
            get
            {
                if (this.PartitionByValue.HasValue())
                {
                   return  "SELECT {0}{" + UtilConstants.ReplaceKey + "} FROM {1}{2}{3}{4}";
                }
                return base.SqlTemplate;
            }
        }
        public override string PageTempalte
        {
            get
            {
                /*
                 SELECT * FROM TABLE WHERE CONDITION ORDER BY ID DESC LIMIT 10 offset 0
                 */
                if (this.PartitionByValue.HasValue()) 
                {
                    return base.PageTempalte;
                }
                var template = "SELECT {0} FROM {1} {2} {3} {4} LIMIT {6} offset {5}";
                return template;
            }
        }
        public override string DefaultOrderByTemplate
        {
            get
            {
                return "ORDER BY NOW() ";
            }
        }

        #endregion

        #region Common Methods
        public override bool IsComplexModel(string sql)
        {
            return Regex.IsMatch(sql, @"AS ""\w+\.\w+""")|| Regex.IsMatch(sql, @"AS ""\w+\.\w+\.\w+""");
        }
        public override string ToSqlString()
        {
            if (PartitionByValue.HasValue()) 
            {
                if (this.Context?.CurrentConnectionConfig?.MoreSettings?.DatabaseModel == DbType.SqlServer)
                {
                    return base.ToSqlString();
                }
                else 
                {
                    return base.ToSqlString().Replace(" GetDate() ", " NOW() ");
                }
            }
            base.AppendFilter();
            string oldOrderValue = this.OrderByValue;
            var isNullOrderValue = Skip == 0 && Take == 1 && oldOrderValue == "ORDER BY NOW() ";
            if (isNullOrderValue)
            {
                this.OrderByValue = null;
            }
            string result = null;
            sql = new StringBuilder();
            sql.AppendFormat(SqlTemplate, GetSelectValue, GetTableNameString, GetWhereValueString, GetGroupByString + HavingInfos, (Skip != null || Take != null) ? null : GetOrderByString);
            if (IsCount) { return sql.ToString(); }
            if (Skip != null && Take == null)
            {
                if (this.OrderByValue == "ORDER BY ") this.OrderByValue += GetSelectValue.Split(',')[0];
                result = string.Format(PageTempalte, GetSelectValue, GetTableNameString, GetWhereValueString, GetGroupByString + HavingInfos, (Skip != null || Take != null) ? null : GetOrderByString, Skip.ObjToInt(), long.MaxValue);
            }
            else if (Skip == null && Take != null)
            {
                if (this.OrderByValue == "ORDER BY ") this.OrderByValue += GetSelectValue.Split(',')[0];
                result = string.Format(PageTempalte, GetSelectValue, GetTableNameString, GetWhereValueString, GetGroupByString + HavingInfos, GetOrderByString, 0, Take.ObjToInt());
            }
            else if (Skip != null && Take != null)
            {
                if (this.OrderByValue == "ORDER BY ") this.OrderByValue += GetSelectValue.Split(',')[0];
                result = string.Format(PageTempalte, GetSelectValue, GetTableNameString, GetWhereValueString, GetGroupByString + HavingInfos, GetOrderByString, Skip.ObjToInt() > 0 ? Skip.ObjToInt() : 0, Take);
            }
            else
            {
                result = sql.ToString();
            }
            this.OrderByValue = oldOrderValue;
            result = GetSqlQuerySql(result);
            if (result.IndexOf("-- No table") > 0)
            {
                return "-- No table";
            }
            if (TranLock != null)
            {
                result = result + TranLock;
            }
            //if (result.Contains("uuid_generate_v4()"))
            //{
            //    result = " CREATE EXTENSION IF NOT EXISTS kbcrypto;CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"; " + result;
            //}
            return result;
        }

        #endregion

        #region Get SQL Partial
        public override string GetExternalOrderBy(string externalOrderBy)
        {
            return Regex.Replace(externalOrderBy, @"""\w+""\.", "");
        }

        public override string GetSelectValue
        {
            get
            {
                string result = string.Empty;
                if (this.SelectValue == null || this.SelectValue is string)
                {
                    result = GetSelectValueByString();
                }
                else
                {
                    result = GetSelectValueByExpression();
                }
                if (this.SelectType == ResolveExpressType.SelectMultiple)
                {
                    this.SelectCacheKey = this.SelectCacheKey + string.Join("-", this.JoinQueryInfos.Select(it => it.TableName));
                }
                if (IsDistinct)
                {
                    result = " DISTINCT " + result;
                }
                if (this.SubToListParameters != null && this.SubToListParameters.Any())
                {
                    result = SubToListMethod(result);
                }
                return result;
            }
        }

        #endregion
    }
}
