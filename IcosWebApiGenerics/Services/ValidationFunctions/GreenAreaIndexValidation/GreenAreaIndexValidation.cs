using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.GreenAreaIndexValidation
{
    public class GreenAreaIndexValidation
    {
       
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static async Task ValidateGaiResponseAsync(GRP_GAI gai, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(gai.GAI_PLOT, "GAI_PLOT", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_PLOT", "$V0$", "GAI_PLOT", "$GRP$", "GRP_GAI");
            }
            else
            {
                //validation of plot reg ex
                if (!GeneralValidation.IsValidPlotString(gai.GAI_PLOT, gai.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_PLOT", "$V0$", "GAI_PLOT", "$V1$", gai.GAI_PLOT);
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(gai.GAI_METHOD, "GAI_METHOD", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_METHOD", "$V0$", "GAI_METHOD", "$GRP$", "GRP_GAI");
            }
            else
            {
                //in badm list
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(gai.GAI_METHOD, (int)Globals.CvIndexes.GAIMETHOD, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_METHOD", "$V0$", gai.GAI_METHOD, "$V1$", "GAI_METHOD", "$GRP$", "GRP_GAI");
                }*/
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(gai.GAI_DATE, "GAI_DATE", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_DATE", "$V0$", "GAI_DATE", "$GRP$", "GRP_GAI");
            }
            errorCode = DatesValidator.IsoDateCheck(gai.GAI_DATE, "GAI_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_DATE", "$V0$", "GAI_DATE", "$V1$", gai.GAI_DATE);
            }

            //get by site id!!!
            errorCode = ValidateGaiByMethod(gai, Ecosystem);
            //return response;
        }

        public static void ValidateCeptResponse(GRP_CEPT cept, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(cept.CEPT_DATE, "CEPT_DATE", "GRP_CEPT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "CEPT_DATE", "$V0$", "CEPT_DATE", "$GRP$", "GRP_CEPT");
            }
            errorCode = DatesValidator.IsoDateCheck(cept.CEPT_DATE, "CEPT_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "CEPT_DATE", "$V0$", "CEPT_DATE", "$V1$", cept.CEPT_DATE);
            }
           // return response;
        }


        public static async Task ValidateBulkhResponseAsync(GRP_BULKH bulkh, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(bulkh.BULKH_DATE, "BULKH_DATE", "GRP_BULKH");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_DATE", "$V0$", "BULKH_DATE", "$GRP$", "GRP_BULKH");
            }
            errorCode = DatesValidator.IsoDateCheck(bulkh.BULKH_DATE, "BULKH_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_DATE", "$V0$", "BULKH_DATE", "$V1$", bulkh.BULKH_DATE);
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(bulkh.BULKH_PLOT, "BULKH_PLOT", "GRP_BULKH");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_PLOT", "$V0$", "BULKH_PLOT", "$GRP$", "GRP_BULKH");
            }
            else
            {
                if (!GeneralValidation.IsValidPlotString(bulkh.BULKH_PLOT, bulkh.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_PLOT", "$V0$", "BULKH_PLOT", "$V1$", bulkh.BULKH_PLOT);
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(bulkh.BULKH_PLOT_TYPE, "BULKH_PLOT_TYPE", "GRP_BULKH");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_PLOT_TYPE", "$V0$", "BULKH_PLOT_TYPE", "$GRP$", "GRP_BULKH");
            }
            else
            {
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(bulkh.BULKH_PLOT_TYPE, (int)Globals.CvIndexes.PLOTTYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_PLOT_TYPE", "$V0$", bulkh.BULKH_PLOT_TYPE, "$V1$", "BULKH_PLOT_TYPE", "$GRP$", "GRP_BULKH");
                }*/
            }

            errorCode = GeneralValidation.MissingMandatoryData<decimal>(bulkh.BULKH, "BULKH", "GRP_BULKH");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH", "$V0$", "BULKH", "$GRP$", "GRP_BULKH");
            }
           //return response;
        }

        ////////////////////////
        ///
        private static int ValidateGaiByMethod(GRP_GAI model, string ecosystem)
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
        }
    }
}
