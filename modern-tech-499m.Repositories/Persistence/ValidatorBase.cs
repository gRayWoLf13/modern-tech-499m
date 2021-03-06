﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace modern_tech_499m.Repositories.Persistence
{
    public abstract class ValidatorBase : IDataErrorInfo
    {
        string IDataErrorInfo.Error => throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");

        public string this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }
                string error = string.Empty;
                var value = GetValue(propertyName);
                var results = new List<ValidationResult>(1);
                var result = Validator.TryValidateProperty(
                    value,
                    new ValidationContext(this, null, null)
                    {
                        MemberName = propertyName
                    },
                    results);
                if (result) 
                    return error;
                var validationResult = results.First();
                error = validationResult.ErrorMessage;
                return error;
            }
        }
        private object GetValue(string propertyName)
        {
            return GetType().GetProperty(propertyName)?.GetValue(this);
        }
    }
}
