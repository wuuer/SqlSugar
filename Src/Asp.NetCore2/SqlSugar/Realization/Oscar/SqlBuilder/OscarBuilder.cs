﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlSugar
{
    public class OscarBuilder : SqlBuilderProvider
    {
        public override string SqlTranslationLeft
        {
            get
            {
                return "\"";
            }
        }
        public override string SqlTranslationRight
        {
            get
            {
                return "\"";
            }
        }
        public override string SqlDateNow
        {
            get
            {
                return "current_date";
            }
        }
        public override string FullSqlDateNow
        {
            get
            {
                return "select current_date";
            }
        }

        public bool isAutoToUpper
        {
            get
            {
                if (this.Context?.CurrentConnectionConfig?.MoreSettings == null) return true;
                return this.Context.CurrentConnectionConfig.MoreSettings.IsAutoToUpper;
            }
        }
        public override string GetTranslationColumnName(string propertyName)
        {
            if (propertyName.Contains(".") && !propertyName.Contains(SqlTranslationLeft))
            {
                return string.Join(".", propertyName.Split('.').Select(it => $"{SqlTranslationLeft}{it.ToUpper(isAutoToUpper)}{SqlTranslationRight}"));
            }
            if (propertyName.Contains(SqlTranslationLeft)) return propertyName;
            else
                return SqlTranslationLeft + propertyName.ToUpper(isAutoToUpper) + SqlTranslationRight;
        }

        //public override string GetNoTranslationColumnName(string name)
        //{
        //    return name.TrimEnd(Convert.ToChar(SqlTranslationRight)).TrimStart(Convert.ToChar(SqlTranslationLeft)).ToLower();
        //}
        public override string GetTranslationColumnName(string entityName, string propertyName)
        {
            Check.ArgumentNullException(entityName, string.Format(ErrorMessage.ObjNotExist, "Table Name"));
            Check.ArgumentNullException(propertyName, string.Format(ErrorMessage.ObjNotExist, "Column Name"));
            var context = this.Context;
            var mappingInfo = context
                 .MappingColumns
                 .FirstOrDefault(it =>
                 it.EntityName.Equals(entityName, StringComparison.CurrentCultureIgnoreCase) &&
                 it.PropertyName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            return (mappingInfo == null ? SqlTranslationLeft + propertyName.ToUpper(isAutoToUpper) + SqlTranslationRight : SqlTranslationLeft + mappingInfo.DbColumnName.ToUpper(isAutoToUpper) + SqlTranslationRight);
        }

        public override string GetTranslationTableName(string name)
        {
            Check.ArgumentNullException(name, string.Format(ErrorMessage.ObjNotExist, "Table Name"));
            var context = this.Context;

            var mappingInfo = context
                .MappingTables
                .FirstOrDefault(it => it.EntityName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            name = (mappingInfo == null ? name : mappingInfo.DbTableName);
            if (name.Contains(".")&& !name.Contains("("))
            {
                return string.Join(".", name.ToUpper(isAutoToUpper).Split('.').Select(it => SqlTranslationLeft + it + SqlTranslationRight));
            }
            else if (name.Contains("("))
            {
                return name;
            }
            else if (name.Contains(SqlTranslationLeft) && name.Contains(SqlTranslationRight))
            {
                return name;
            }
            else
            {
                return SqlTranslationLeft + name.ToUpper(isAutoToUpper).TrimEnd('"').TrimStart('"') + SqlTranslationRight;
            }
        }
    }
}
