using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Services.ValidationFunctions;
using IcosWebApiGenerics.Services.ValidationFunctions.StationDescValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.GreenAreaIndexValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.ECValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.DataRecordValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.SamplingValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.STALValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.MeteoValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.StorageValidation;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IcosWebApiGenerics.Services
{
    public class ValidatorService<T> : IValidatorService<T> where T : BaseClass, new()
    {
        private readonly IcosDbContext _context;
        private Response response;
        public ValidatorService(IcosDbContext context)
        {
            this._context = context;
            response= GetResponse();
            response.Code = 0;
            response.Messages.Clear();
        }
        public Response GetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            return response;
        }

        /* public int ValidateModel(T t)
         {
             int res = 0;
             switch (t.GroupId)
             {
                 case (int)Globals.Groups.GRP_LOCATION: //grp location
                     res = ValidateLocationGroup(t);
                     break;
                 case (int)Globals.Groups.GRP_UTC_OFFSET:
                     res = ValidateUtcOffsetGroup(t);
                     break;
             }
             return res;
         }*/

        public Response ItemInBadmList(string value, int cvIndex)
        {
            string bmList = _context.BADMList.Where(item => item.cv_index == cvIndex).Select(x => x.vocabulary).FirstOrDefault();
            var res = _context.BADMList.Where(item => item.cv_index == cvIndex)
                                       .Any(item => (String.Compare(item.shortname, value, true) == 0));
            if (!res)
            {

            }

            return response;
        }

        public async Task<Response> Validate(T t)
        {
            //May be to add a general validation? For Dates format? Mandatory variables? Item in CV?
            var xList = await GetCvIndexVariables(t.GroupId);
            if (xList != null)
            {
                var props = t.GetType().GetProperties();
                foreach (var xl in xList)
                {
                    var prop = props.FirstOrDefault(p => p.Name == xl.Name);
                    string value;
                    try
                    {
                        var sv = prop.GetValue(t, null).ToString();
                        value = (string)sv;
                    }
                    catch(Exception e)
                    {
                        continue;
                    }
                   // var value = prop.GetValue(t, null).ToString();
                    string str = "";
                    if (!String.IsNullOrEmpty((string)value))
                    {
                        int xCv = IsInControlledVocabularyExt(value, (int)xl.CvIndex, ref str);
                        if (xCv == 0)
                        {
                            if (!String.IsNullOrEmpty(str))
                            {
                                prop.SetValue(t, str);
                            }
                        }
                        else//if (!IsInControlledVocabulary(value, (int)xl.CvIndex))
                        {
                            response.Code += xCv;
                            response.FormatError(ErrorCodes.GeneralErrors[7], prop.Name, "$V0$", value, "$V1$", BadmListName((int)xl.CvIndex), "$GRP$", GetGroupName(t.GroupId));
                        }
                    }
                }
            }

            //Dates....
            var _dates = GetDatesVariables(t.GroupId);
            if (_dates != null)
            {
                var props = t.GetType().GetProperties();
                foreach (var _dt in _dates)
                {
                    var prop = props.FirstOrDefault(p => p.Name == _dt.Name);
                    var value = prop.GetValue(t, null).ToString();
                    //Dates not in ISODATE format: for example, dd-mm-yyyy or dd/mm//yyyy:
                    //try to convert in ISODATE
                    if ((value.IndexOf("/") >= 0) || (value.IndexOf("-") >= 0) || (value.IndexOf(" ") >= 0))
                    {
                        try
                        {
                            string temp = value;
                            value = DatesValidator.DateTransform(value);
                            prop.SetValue(t, value);
                            response.FormatWarnings("Warning: wrong date format in cell " + "date.Cell" + ": " + temp + " converted in " + value, 0);
                        }
                        catch (Exception e)
                        {
                            response.Code += 2;
                            response.FormatError(ErrorCodes.GeneralErrors[2], prop.Name, "$V0$", prop.Name, "$V1$", value);
                        }
                    }
                    else
                    {
                        //is a valid isodate?
                        int dVal = DatesValidator.IsoDateCheck(value, prop.Name);
                        if (dVal > 0)
                        {
                            response.Code += dVal;
                            response.FormatError(ErrorCodes.GeneralErrors[dVal], prop.Name, "$V0$", prop.Name, "$V1$", value);
                        }
                    }
                }
            }


            switch (t.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION:
                    GRP_LOCATION location = t as GRP_LOCATION;
                    StationDescValidation.ValidateLocationResponse(location, response);
                    break;
                case (int)Globals.Groups.GRP_UTC_OFFSET:
                    GRP_UTC_OFFSET utc = t as GRP_UTC_OFFSET;
                    StationDescValidation.ValidateUtcResponse(utc, response);
                    break;
                case (int)Globals.Groups.GRP_LAND_OWNERSHIP:
                    GRP_LAND_OWNERSHIP land = t as GRP_LAND_OWNERSHIP;
                    await StationDescValidation.ValidateLandOwnerResponseAsync(land, _context, response);
                    break;
                case (int)Globals.Groups.GRP_TOWER:
                    GRP_TOWER tower = t as GRP_TOWER;
                    await StationDescValidation.ValidateTowerResponseAsync(tower, _context, response);
                    break;
                case (int)Globals.Groups.GRP_CLIM_AVG:
                    GRP_CLIM_AVG climAvg = t as GRP_CLIM_AVG;
                    StationDescValidation.ValidateClimateAvgResponse(climAvg, response);
                    break;
                case (int)Globals.Groups.GRP_DM:
                    GRP_DM distManager = t as GRP_DM;
                    StationDescValidation.ValidateDistManResponse(distManager, _context, response);
                    break;
                case (int)Globals.Groups.GRP_PLOT:
                    GRP_PLOT samplingScheme = t as GRP_PLOT;
                    await SamplingValidation.ValidateSamplingSchemeResponseAsync(samplingScheme, _context, response);
                    break;
                case (int)Globals.Groups.GRP_FLSM:
                    GRP_FLSM flsm = t as GRP_FLSM;
                    await SamplingValidation.ValidateFlsmResponseAsync(flsm, _context, response);
                    break;
                case (int)Globals.Groups.GRP_SOSM:
                    GRP_SOSM sosm = t as GRP_SOSM;
                    SamplingValidation.ValidateSosmResponse(sosm, _context, response);
                    break;
                case (int)Globals.Groups.GRP_DHP:
                    GRP_DHP dhp = t as GRP_DHP;
                    await GrpValidator.ValidateDhpResponseAsync(dhp, _context, response);
                    break;
                case (int)Globals.Groups.GRP_GAI:
                    GRP_GAI gai = t as GRP_GAI;
                    await GreenAreaIndexValidation.ValidateGaiResponseAsync(gai, _context, response);
                    break;
                case (int)Globals.Groups.GRP_CEPT:
                    GRP_CEPT cept = t as GRP_CEPT;
                    GreenAreaIndexValidation.ValidateCeptResponse(cept, response);
                    break;
                case (int)Globals.Groups.GRP_BULKH:
                    GRP_BULKH bulkh = t as GRP_BULKH;
                    await GreenAreaIndexValidation.ValidateBulkhResponseAsync(bulkh, _context, response);
                    break;
                case (int)Globals.Groups.GRP_SPPS:
                    GRP_SPPS spps = t as GRP_SPPS;
                    await STALValidation.ValidateSppsResponseAsync(spps, _context, response);
                    break;
                case (int)Globals.Groups.GRP_TREE:
                    GRP_TREE dSnow = t as GRP_TREE;
                    STALValidation.ValidateTreeResponse(dSnow, _context, response);
                    break;
                case (int)Globals.Groups.GRP_AGB:
                    GRP_AGB agb = t as GRP_AGB;
                    STALValidation.ValidateAgbResponse(agb, _context, response);
                    break;
                case (int)Globals.Groups.GRP_LITTERPNT:
                    GRP_LITTERPNT litterPnt = t as GRP_LITTERPNT;
                    STALValidation.ValidateLitterPntResponse(litterPnt, _context, response);
                    break;
                case (int)Globals.Groups.GRP_ALLOM:
                    GRP_ALLOM allom = t as GRP_ALLOM;
                    STALValidation.ValidateAllomResponse(allom, _context, response);
                    break;
                case (int)Globals.Groups.GRP_WTDPNT:
                    GRP_WTDPNT wtdPnt = t as GRP_WTDPNT;
                    GrpValidator.ValidateWtdPntResponse(wtdPnt, _context, response);
                    break;
                case (int)Globals.Groups.GRP_D_SNOW:
                    GRP_D_SNOW d_Snow = t as GRP_D_SNOW;
                    GrpValidator.ValidateDSnowResponse(d_Snow, _context, response);
                    break;
                case (int)Globals.Groups.GRP_INST:
                    GRP_INST inst = t as GRP_INST;
                    await GrpValidator.ValidateInstResponseAsync(inst, _context, response);
                    break;
                case (int)Globals.Groups.GRP_LOGGER:
                    GRP_LOGGER logger = t as GRP_LOGGER;
                    await DataRecordValidation.ValidateLoggerResponseAsync(logger, _context, response);
                    break;
                case (int)Globals.Groups.GRP_FILE:
                    GRP_FILE _file = t as GRP_FILE;
                    await DataRecordValidation.ValidateFileResponseAsync(_file, _context, response);
                    break;
                case (int)Globals.Groups.GRP_EC:
                    GRP_EC ecInst = t as GRP_EC;
                    await ECValidation.ValidateEcResponseAsync(ecInst, _context, response);
                    break;
                case (int)Globals.Groups.GRP_ECSYS:
                    GRP_ECSYS ecSys = t as GRP_ECSYS;
                    response = await ECValidation.ValidateEcSysResponseAsync(ecSys, _context, response);
                    break;
                case (int)Globals.Groups.GRP_ECWEXCL:
                    GRP_ECWEXCL ecWexcl = t as GRP_ECWEXCL;
                    response = await ECValidation.ValidateEcWexclResponseAsync(ecWexcl, _context, response);
                    break;
                case (int)Globals.Groups.GRP_BM:
                    GRP_BM bmModel = t as GRP_BM;
                    await MeteoValidation.ValidateBmResponseAsync(bmModel, _context, response);
                    break;
                case (int)Globals.Groups.GRP_STO:
                    GRP_STO stModel = t as GRP_STO;
                    response = await StorageValidation.ValidateStorageResponseAsync(stModel, _context);
                    break;
            }
            return response;
        }

        private async Task<IEnumerable<Variable>> GetCvIndexVariables(int grId)
        {
            var variables = await _context.Variables.Where(vv => vv.GroupId == grId && (vv.CvIndex > 0 /*&& vv.CvIndex != null*/)).ToListAsync();
           // Console.WriteLine(query.ToQueryString());
           // List<Variable> variables = query.ToList();
            return variables;
        }

        private IEnumerable<Variable> GetDatesVariables(int grId)
        {
            var query = _context.Variables.Where(vv => vv.GroupId == grId && vv.Name.IndexOf("DATE") >= 0 && vv.Name.IndexOf("DATE_UNC") < 0);
            Console.WriteLine(query.ToQueryString());
            List<Variable> variables = query.ToList();
            return variables;
        }

        private int IsInControlledVocabularyExt(string value, int cvIndex, ref string newVal)
        {
            var item = _context.BADMList.Where(bm => bm.cv_index == cvIndex && bm.shortname == value).FirstOrDefault();//.shortname;
            if (item == null) return 7;
            if (String.Compare(item.shortname, value, false) != 0)
            {
                response.Warnings.Add(response.WarningCode++, "Warning: case differences in entered item. Found " +value + " instead of " + item.shortname + " in cell " + "v.Cell" + ". Item value will be corrected");
                newVal = item.shortname;
            }
            newVal = null;
            return 0;
        }

        private string BadmListName(int cvIndex)
        {
            var item = _context.Variables.Where(bm => bm.CvIndex == cvIndex).FirstOrDefault();//.shortname;
            
            return item.UnitOfMeasure;
        }

        private string GetGroupName(int groupId)
        {
            var item = _context.Groups.Where(gr=>gr.id_group==groupId).FirstOrDefault();//.shortname;
            return item.GroupName;
        }
    }
}
