using IcosWebApi.Models.Obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IcosWebApi.Models
{
    public class Mapper : IMapper
    {
       // private readonly VarForGroup _vgGroup;
        private readonly IErrorLogger _errLogger;
        private string ErrorMessage { get; set; }
        private BaseClass _baseObj;
        public int MapResult { get; set; }
        public Mapper(IErrorLogger logger)
        {
            //_vgGroup = vgGroup;
            _errLogger = logger;
        }
        public BaseClass MapToObject(VarForGroup _vgGroup)
        {
            switch (_vgGroup.GroupId)
            {
                case 3:
                    _baseObj = new GRP_LOCATION();
                    break;
            }

            Type myType = _baseObj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            var subList = props.Where(item => item.Name != "Id" &&
                                               item.Name != "DataStatus" &&
                                               !item.Name.Contains("UserId") &&
                                               !item.Name.Contains("Date") &&
                                               !item.Name.Contains("SiteId") &&
                                               !item.Name.Contains("GroupId") &&
                                               !item.Name.Contains("DataOrigin")).ToList();
            foreach (var prop in subList)
            {
                var variable = _vgGroup.VList.Find(vv => vv.Name == prop.Name);
                if (variable == null) continue;
                //prop.SetValue(_baseObj, variable.Value);
                if (SetValue(prop, variable) < 0)
                {
                    ErrorMessage = "\r\nError: cell " + variable.Cell + ": " + ErrorMessage;
                    _errLogger.LogErrorMessage(ErrorMessage);
                    MapResult = 1;
                }
                var item = prop.GetValue(_baseObj, null);
            }
            return _baseObj;
        }

        private int SetValue(PropertyInfo prop, Variable vv)
        {

            switch (vv.Unit)
            {
                case 1:
                case 5:
                    prop.SetValue(_baseObj, vv.Value);
                    break;
                case 2:
                    try
                    {
                        prop.SetValue(_baseObj, Convert.ToDecimal(vv.Value));
                    }
                    catch (Exception e)
                    {
                        prop.SetValue(_baseObj, -9999.0m);
                        ErrorMessage = "Wrong decimal value format for variable " + prop.Name + ": found " + vv.Value;
                        return -1;
                    }
                    break;
                case 3:
                case 4:
                    try
                    {
                        prop.SetValue(_baseObj, Convert.ToInt32(vv.Value));
                    }
                    catch (Exception e)
                    {
                        prop.SetValue(_baseObj, -9999);
                        ErrorMessage = "Wrong integer value format for variable " + prop.Name + ": found " + vv.Value;
                        return -1;
                    }
                    break;
            }
            return 0;
        }
    }
}
