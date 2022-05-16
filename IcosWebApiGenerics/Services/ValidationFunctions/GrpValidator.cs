using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public class GrpValidator
    {
        private Response response;
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        

        public static async Task ValidateDhpResponseAsync(GRP_DHP dhp, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<int>(dhp.DHP_ID, "DHP_ID", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_ID", "$V0$", "DHP_ID", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be integer value...
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(dhp.DHP_CAMERA, "DHP_CAMERA", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA", "$V0$", "DHP_CAMERA", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be in badmlist
                errorCode = await GeneralValidation.ItemInBadmListAsync(dhp.DHP_CAMERA, (int)Globals.CvIndexes.CAMERA, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA", "$V0$", dhp.DHP_CAMERA, "$V1$", "CAMERA", "$GRP$", "GRP_DHP");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(dhp.DHP_CAMERA_SN, "DHP_CAMERA_SN", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA_SN", "$V0$", "DHP_CAMERA_SN", "$GRP$", "GRP_DHP");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(dhp.DHP_LENS, "DHP_LENS", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS", "$V0$", "DHP_LENS", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be in badmlist
                errorCode = await GeneralValidation.ItemInBadmListAsync(dhp.DHP_LENS, (int)Globals.CvIndexes.LENS, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS", "$V0$", dhp.DHP_LENS, "$V1$", "LENS", "$GRP$", "GRP_DHP");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(dhp.DHP_LENS_SN, "DHP_LENS_SN", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS_SN", "$V0$", "DHP_LENS_SN", "$GRP$", "GRP_DHP");
            }

            bool isRowCol = true;
            errorCode = GeneralValidation.MissingMandatoryData<int>(dhp.DHP_OC_ROW, "DHP_OC_ROW", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_OC_ROW", "$V0$", "DHP_OC_ROW", "$GRP$", "GRP_DHP");
                isRowCol = false;
            }
            else
            {
                //must be integer value...
            }

            errorCode = GeneralValidation.MissingMandatoryData<int>(dhp.DHP_OC_COL, "DHP_OC_COL", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_OC_COL", "$V0$", "DHP_OC_COL", "$GRP$", "GRP_DHP");
                isRowCol = false;
            }
            else
            {
                //must be integer value...
            }

            if(isRowCol)
            {
                if (dhp.DHP_OC_COL <= dhp.DHP_OC_ROW)
                {
                    errorCode = 1;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpDhpErrors[errorCode], "DHP_OC_COL");
                }
             
            }

            errorCode = GeneralValidation.MissingMandatoryData<decimal>(dhp.DHP_RADIUS, "DHP_RADIUS", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_RADIUS", "$V0$", "DHP_RADIUS", "$GRP$", "GRP_DHP");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(dhp.DHP_DATE, "DHP_DATE", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_DATE", "$V0$", "DHP_DATE", "$GRP$", "GRP_DHP");
            }
            errorCode = DatesValidator.IsoDateCheck(dhp.DHP_DATE, "DHP_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_DATE", "$V0$", "DHP_DATE", "$V1$", dhp.DHP_DATE);
            }

            //return response;
        }

        public static void ValidateDSnowResponse(GRP_D_SNOW dSnow, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(dSnow.D_SNOW_DATE, "D_SNOW_DATE", "GRP_D_SNOW");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "D_SNOW_DATE", "$V0$", "D_SNOW_DATE", "$GRP$", "GRP_D_SNOW");
            }
            errorCode = DatesValidator.IsoDateCheck(dSnow.D_SNOW_DATE, "D_SNOW_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "D_SNOW_DATE", "$V0$", "D_SNOW_DATE", "$V1$", dSnow.D_SNOW_DATE);
            }

            if (Globals.IsValidCoordinateSystem(dSnow.D_SNOW_EASTWARD_DIST, dSnow.D_SNOW_NORTHWARD_DIST,
                                                dSnow.D_SNOW_DISTANCE_POLAR, dSnow.D_SNOW_ANGLE_POLAR) > 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpD_SnowErrors[errorCode], "D_SNOW_EASTWARD_DIST");
            }
            if (!GeneralValidation.XORNull<string>(dSnow.D_SNOW_PLOT, dSnow.D_SNOW_VARMAP))
            {
                errorCode = (int)Globals.ErrorValidationCodes.D_SNOW_PLOT_VARMAP;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpD_SnowErrors[errorCode], "D_SNOW_PLOT");
            }

            if (!String.IsNullOrEmpty(dSnow.D_SNOW_PLOT))
            {
                errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(dSnow.D_SNOW_PLOT, dSnow.D_SNOW_DATE, dSnow.SiteId, db);
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "D_SNOW_PLOT");
                }
            }
            //return response;
        }

        public static void ValidateWtdPntResponse(GRP_WTDPNT wtdPnt, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(wtdPnt.WTDPNT_DATE, "WTDPNT_DATE", "GRP_WTDPNT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "WTDPNT_DATE", "$V0$", "WTDPNT_DATE", "$GRP$", "GRP_WTDPNT");
            }
            errorCode = DatesValidator.IsoDateCheck(wtdPnt.WTDPNT_DATE, "WTDPNT_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "WTDPNT_DATE", "$V0$", "WTDPNT_DATE", "$V1$", wtdPnt.WTDPNT_DATE);
            }

            if (Globals.IsValidCoordinateSystem(wtdPnt.WTDPNT_EASTWARD_DIST, wtdPnt.WTDPNT_NORTHWARD_DIST,
                                                wtdPnt.WTDPNT_DISTANCE_POLAR, wtdPnt.WTDPNT_ANGLE_POLAR) > 0)
            {
                errorCode = (int)Globals.ErrorValidationCodes.INVALID_COORDINATE_SYSTEM;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpWtdPntErrors[errorCode], "WTDPNT_EASTWARD_DIST");
            }

            if (!GeneralValidation.XORNull<string>(wtdPnt.WTDPNT_PLOT, wtdPnt.WTDPNT_VARMAP))
            {
                errorCode = (int)Globals.ErrorValidationCodes.WTDPNT_PLOT_VARMAP;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpWtdPntErrors[errorCode], "WTDPNT_PLOT");
            }

            if (!String.IsNullOrEmpty(wtdPnt.WTDPNT_PLOT))
            {
                errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(wtdPnt.WTDPNT_PLOT, wtdPnt.WTDPNT_DATE, wtdPnt.SiteId, db);
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "WTDPNT_PLOT");
                }
            }

            //return response;
        }

        public static async Task ValidateInstResponseAsync(GRP_INST inst, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(inst.INST_MODEL, "INST_MODEL", "GRP_INST");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_MODEL", "$V0$", "INST_MODEL", "$GRP$", "GRP_INST");
            }
            else
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(inst.INST_MODEL, (int)Globals.CvIndexes.INST_MODEL, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_MODEL", "$V0$", inst.INST_MODEL, "$V1$", "INST_MODEL", "$GRP$", "GRP_INST");
                }
            }

            
            errorCode = GeneralValidation.MissingMandatoryData<string>(inst.INST_SN, "INST_SN", "GRP_INST");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_SN", "$V0$", "INST_SN", "$GRP$", "GRP_INST");
            }
            else
            {
                errorCode = InstrumentsValidation.SerialNumberCheck(inst.INST_MODEL, inst.INST_SN);
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_SN", "$V0$", "INST_SN", "$GRP$", "GRP_INST");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(inst.INST_FACTORY, "INST_FACTORY", "GRP_INST");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_FACTORY", "$V0$", "INST_FACTORY", "$GRP$", "GRP_INST");
            }
            else
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(inst.INST_FACTORY, (int)Globals.CvIndexes.INST_FACTORY, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_FACTORY", "$V0$", inst.INST_MODEL, "$V1$", "INST_FACTORY", "$GRP$", "GRP_INST");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(inst.INST_DATE, "INST_DATE", "GRP_INST");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_DATE", "$V0$", "INST_DATE", "$GRP$", "GRP_INST");
            }
            errorCode = DatesValidator.IsoDateCheck(inst.INST_DATE, "INST_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "INST_DATE", "$V0$", "INST_DATE", "$V1$", inst.INST_DATE);
            }

            errorCode = await InstrumentsValidation.InstrumentInGrpInst(inst, inst.SiteId, db);
            if (errorCode > 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpInstErrors[errorCode], "INST_MODEL");
            }

            errorCode = await InstrumentsValidation.LastExpectedOpByDateAsync(inst, db);
            if (errorCode > 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpInstErrors[errorCode], "INST_FACTORY");
            }
            //return response;
        }
        
        /////////////////////////////////
        
        /*private static int ValidateGaiByMethod(GRP_GAI model,string ecosystem)
        {
            //clean all data not bound to selected method...best to do before validation???

            bool checkCoordinates = false;
            switch (model.GAI_METHOD.ToLower())
            {
                //can be used only for Forests

                case "dhp":
                    if (String.Compare(ecosystem, "Forest", true) != 0)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_DHP_ONLY_FOREST;
                    }
                    if (model.GAI_DHP_ID == null)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_DHP_MANDATORY_MISSING;
                    }
                    if (model.GAI_DHP_SLOPE == null)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_DHP_MANDATORY_MISSING;
                    }
                    if (model.GAI_DHP_ASPECT == null)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_DHP_MANDATORY_MISSING;
                    }
                    if (model.GAI_PLOT.StartsWith("CP"))
                    {
                        if (model.GAI_DHP_EASTWARD_DIST == null && model.GAI_DHP_NORTHWARD_DIST == null
                            && model.GAI_DHP_DISTANCE_POLAR == null && model.GAI_DHP_ANGLE_POLAR == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_DHP_MISSING_COORDINATES;
                        }
                        else
                        {
                            checkCoordinates = true;
                        }
                    }
                    else
                    {
                        if (model.GAI_DHP_EASTWARD_DIST != null || model.GAI_DHP_NORTHWARD_DIST != null
                            || model.GAI_DHP_DISTANCE_POLAR != null || model.GAI_DHP_ANGLE_POLAR != null)
                        {
                            checkCoordinates = true;
                        }
                    }
                    if (checkCoordinates)
                    {
                        if (Globals.IsValidCoordinateSystem<decimal?>(model.GAI_DHP_EASTWARD_DIST, model.GAI_DHP_NORTHWARD_DIST, model.GAI_DHP_DISTANCE_POLAR, model.GAI_DHP_ANGLE_POLAR) > 0)
                        {
                            return (int)Globals.ErrorValidationCodes.INVALID_COORDINATE_SYSTEM;
                        }
                    }


                    break;
                case "destructive":
                    if (String.Compare(ecosystem, "Grassland", true) == 0)
                    {
                        if (model.GAI == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_MISSING_GRASSLAND;
                        }
                        if (model.GAI_AREA == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_AREA_MISSING_GRASSLAND;
                        }
                        if (String.IsNullOrEmpty(model.GAI_PLOT_TYPE))
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_PLOT_TYPE_MISSING_GRASSLAND;
                        }
                        if (!String.IsNullOrEmpty(model.GAI_PTYPE))
                        {
                            if (model.GAI_PTYPE.ToLower() == "weed" && model.GAI_PTYPE.ToLower() == "crop")
                            {
                                return (int)Globals.ErrorValidationCodes.GAI_PTYPE_WEED_CROP_INVALID_GRASSLAND;
                            }
                        }
                    }
                    else if (String.Compare(ecosystem, "Cropland", true) == 0)
                    {
                        if (model.GAI == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_MISSING_GRASSLAND; ;
                        }
                        if (model.GAI_AREA == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_AREA_MISSING_GRASSLAND;
                        }
                        if (model.GAI_LOCATION == null)
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_LOCATION_MISSING_CROPLAND;
                        }
                        if (String.IsNullOrEmpty(model.GAI_PTYPE))
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_PTYPE_MISSING_CROPLAND;
                        }
                        else
                        {
                            if (model.GAI_PTYPE.ToLower() != "weed" && model.GAI_PTYPE.ToLower() != "crop")
                            {
                                return (int)Globals.ErrorValidationCodes.GAI_PTYPE_WEED_CROP_INVALID_CROPLAND;
                            }
                        }
                        if (!String.IsNullOrEmpty(model.GAI_SPP))
                        {
                            if (model.GAI_PTYPE.ToLower() != "crop")
                            {
                                return (int)Globals.ErrorValidationCodes.GAI_SPP_CROP_CROPLAND;
                            }
                        }
                    }
                    else
                    {
                        //return 1011;
                    }
                    break;
                case "visual estimation":
                case "modified vga":
                    //only for Mires, who the f*** knows what they are...
                    if (String.Compare(ecosystem, "Grassland", true) == 0 || String.Compare(ecosystem, "Cropland", true) == 0 || String.Compare(ecosystem, "Forest", true) == 0)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_ONLY_MIRES;
                    }
                    if (model.GAI == null)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_MISSING_SPECTRAL_REF;
                    }
                    if (String.IsNullOrEmpty(model.GAI_SPP))
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_SPP_MISSING_MIRES;
                    }
                    break;
                case "spectral reflectance":
                    if (model.GAI == null)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_MISSING_SPECTRAL_REF;
                    }
                    break;
                case "accupar":
                    if (String.Compare(ecosystem, "Grassland", true) != 0 && String.Compare(ecosystem, "Cropland", true) != 0)
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_ACCUPAR;
                    }
                    if (String.Compare(ecosystem, "Grassland", true) == 0)
                    {
                        if (String.IsNullOrEmpty(model.GAI_PLOT_TYPE))
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_PLOT_TYPE_ACCUPAR_GRASSLAND_MANDATORY;
                        }
                    }
                    break;
                case "sunscan":
                    if (String.Compare(ecosystem, "Forest", true) == 0)
                    {
                        if (model.GAI_PLOT.StartsWith("CP"))
                        {
                            if (model.GAI_CEPT_EASTWARD_DIST == null && model.GAI_CEPT_NORTHWARD_DIST == null
                                && model.GAI_CEPT_DISTANCE_POLAR == null && model.GAI_CEPT_ANGLE_POLAR == null)
                            {
                                return (int)Globals.ErrorValidationCodes.GAI_CEPT_MANDATORY_MISSING;
                            }
                            else
                            {
                                checkCoordinates = true;
                            }
                            if (model.GAI_CEPT_ID == null)
                            {
                                return (int)Globals.ErrorValidationCodes.GAI_CEPT_ID_MANDATORY_MISSING;
                            }
                        }
                        else
                        {
                            if (model.GAI_CEPT_EASTWARD_DIST != null || model.GAI_CEPT_NORTHWARD_DIST != null
                                || model.GAI_CEPT_DISTANCE_POLAR != null || model.GAI_CEPT_ANGLE_POLAR != null)
                            {
                                checkCoordinates = true;
                            }
                        }
                        if (checkCoordinates)
                        {
                            if (Globals.IsValidCoordinateSystem<decimal?>(model.GAI_CEPT_EASTWARD_DIST, model.GAI_CEPT_NORTHWARD_DIST, model.GAI_CEPT_DISTANCE_POLAR, model.GAI_CEPT_ANGLE_POLAR) > 0)
                            {
                                return (int)Globals.ErrorValidationCodes.INVALID_COORDINATE_SYSTEM;
                            }
                        }
                    }
                    else if (String.Compare(ecosystem, "Grassland", true) == 0)
                    {
                        if (String.IsNullOrEmpty(model.GAI_PLOT_TYPE))
                        {
                            return (int)Globals.ErrorValidationCodes.GAI_PLOT_TYPE_SUNSCAN;
                        }
                    }
                    else if (String.Compare(ecosystem, "Cropland", true) == 0)
                    {

                    }
                    else
                    {
                        return (int)Globals.ErrorValidationCodes.GAI_SUNSCAN_ECOSYSTEMS;
                    }
                    break;
            }
            return 0;
        }*/


        public int CheckFileFormat(string fileFormat, string fileExt, int siteId)
        {
            int res = 0;
            if (fileFormat == null && fileExt == null)
            {
                return 0;
            }
            if ((fileFormat != null && fileExt == null) || (fileFormat == null && fileExt != null))
            {
                return (int)Globals.ErrorValidationCodes.FILE_FORMAT_FILEXT;
            }
            switch (fileFormat.ToLower())
            {
                case "binary":
                    if (String.Compare(fileExt.ToLower(), ".csv", true) == 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.CSV_NOT_ALLOWED_FOR_BINARY;
                    }
                    break;
                case "ascii":
                    if (String.Compare(fileExt.ToLower(), ".bin", true) == 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.BIN_NOT_ALLOWED_FOR_ASCII;
                    }
                    break;
            }
            return res;
        }
        public int CheckHeadConstraints(string fileFormat, int? fileHeadNum, int? fileHeadVars, int? fileHeadType)
        {
            int res = 0;
            if (fileFormat == null)
            {
                if (fileHeadNum == null && fileHeadVars == null && fileHeadType == null)
                {
                    return 0;
                }
                else
                {
                    return 555;
                }
            }
            if (String.Compare(fileFormat.ToLower(), "binary", true) == 0)
            {
                
                if (fileHeadNum == null && fileHeadVars == null && fileHeadType == null)
                {
                    return 0;
                }
                if (fileHeadType == null && fileHeadNum != null && fileHeadVars != null)
                {
                    if (fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_TYPE_MISSING_BINARY;
                    }
                    else if (fileHeadType == null && fileHeadNum == null && fileHeadVars != null)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_TYPE_MISSING_BINARY;
                    }
                    else if (fileHeadType != null && fileHeadNum == null && fileHeadVars == null)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_VARS_MISSING_BINARY;
                    }
                    else
                    {

                        if (fileHeadVars > fileHeadNum)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_GT_FILE_HEAD_NUM;
                        }
                        if (fileHeadType > fileHeadNum)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_GT_FILE_HEAD_NUM;
                        }
                        if (fileHeadVars == fileHeadType && fileHeadNum != 0)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_GT_FILE_HEAD_VARS;
                        }
                    }
                }
            }
            else //ASCII
            {
                if (fileHeadNum != null && fileHeadVars == null)
                {
                    if (fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_MISSING;
                    }

                }
                else if (fileHeadNum != null && fileHeadVars == null)
                {
                    //OK....
                }
                else if (fileHeadNum == null && fileHeadVars != null)
                {
                    res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_MISSING;
                }
                else
                {


                    if (fileHeadVars != null && fileHeadVars > fileHeadNum)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_GT_FILE_HEAD_NUM;
                    }
                    if (fileHeadVars == fileHeadType && fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_EQ_FILE_HEAD_VARS;
                    }
                }
            }
            return res;
        }

    }
}
