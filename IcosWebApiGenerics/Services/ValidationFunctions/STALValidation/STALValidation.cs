using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.STALValidation
{
    /// <summary>
    /// SPPS, TREE; ALLOM, LITTERPNT
    /// </summary>
    public class STALValidation
    {
      
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static async Task ValidateSppsResponseAsync(GRP_SPPS spps, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(spps.SPPS_DATE, "SPPS_DATE", "GRP_SPPS");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_DATE", "$V0$", "SPPS_DATE", "$GRP$", "GRP_SPPS");
            }
            errorCode = DatesValidator.IsoDateCheck(spps.SPPS_DATE, "SPPS_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_DATE", "$V0$", "SPPS_DATE", "$V1$", spps.SPPS_DATE);
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(spps.SPPS_PLOT, "SPPS_PLOT", "GRP_SPPS");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_PLOT", "$V0$", "SPPS_PLOT", "$GRP$", "GRP_SPPS");
            }
            else
            {
                if (!GeneralValidation.IsValidPlotString(spps.SPPS_PLOT, spps.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_PLOT", "$V0$", "SPPS_PLOT", "$V1$", spps.SPPS_PLOT);
                }
                //Here?
                errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(spps.SPPS_PLOT, spps.SPPS_DATE, spps.SiteId, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_PLOT");
                }
            }

            bool isBound = false;
            if (Globals.IsValidCoordinateSystem<decimal?>(spps.SPPS_LOCATION_LAT, spps.SPPS_LOCATION_LON, spps.SPPS_LOCATION_DIST, spps.SPPS_LOCATION_ANG) > 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpSppsErrors[errorCode], "SPPS_LOCATION_LAT");
            }

            if (spps.SPPS_LOCATION_LAT != null && spps.SPPS_LOCATION_LON != null)
            {
                isBound = GeneralValidation.CountBoundedProps<GRP_SPPS>(spps, 3, "SPPS_LOCATION_LAT", "SPPS_LOCATION_LON", "SPPS_LOCATION");
            }
            else
            {
                isBound = GeneralValidation.CountBoundedProps<GRP_SPPS>(spps, 3, "SPPS_LOCATION_DIST", "SPPS_LOCATION_ANG", "SPPS_LOCATION");
            }
            if (!isBound)
            {
                errorCode = (int)Globals.ErrorValidationCodes.SPPS_LOCATION_BOUND;
                response.Code += errorCode;
            }
            if (!String.IsNullOrEmpty(spps.SPPS_PTYPE))
            {
                if (!spps.SPPS_PLOT.ToLower().StartsWith("cp"))
                {

                    errorCode = (int)Globals.ErrorValidationCodes.SPPS_PTYPE_CP_ALLOWED;
                    response.Code += errorCode;
                }
            }


            //Only for mires!!! wtf are they??????????
            if (spps.SPPS_PERC_COVER != null)
            {

            }

            if (!String.IsNullOrEmpty(spps.SPPS_TWSP_PCT))
            {
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(spps.SPPS_TWSP_PCT, (int)Globals.CvIndexes.TWSP, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_TWSP_PCT", "$V0$", spps.SPPS_TWSP_PCT, "$V1$", "SPPS_TWSP_PCT", "$GRP$", "GRP_SPPS");
                }*/
            }

            if (!String.IsNullOrEmpty(spps.SPPS_PTYPE))
            {
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(spps.SPPS_PTYPE, (int)Globals.CvIndexes.SPPPTYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_PTYPE", "$V0$", spps.SPPS_PTYPE, "$V1$", "SPPS_PTYPE", "$GRP$", "GRP_SPPS");
                }*/
            }

           // return response;
        }

        public static void ValidateTreeResponse(GRP_TREE tree, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(tree.TREE_DATE, "TREE_DATE", "GRP_TREE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_DATE", "$V0$", "TREE_DATE", "$GRP$", "GRP_TREE");
            }
            errorCode = DatesValidator.IsoDateCheck(tree.TREE_DATE, "TREE_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_DATE", "$V0$", "TREE_DATE", "$V1$", tree.TREE_DATE);
            }

            if (tree.TREE_PLOT.ToLower().StartsWith("cp") || tree.TREE_PLOT.ToLower().EndsWith("cp"))
            {
                if (tree.TREE_ID == null)
                {
                    errorCode = GeneralValidation.MissingMandatoryData<decimal?>(tree.TREE_DBH, "TREE_PLOT", "GRP_TREE");
                    if (errorCode != 0)
                    {
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_PLOT", "$V0$", "TREE_PLOT", "$GRP$", "GRP_TREE");
                    }
                    else
                    {
                        errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(tree.TREE_PLOT, tree.TREE_DATE, tree.SiteId, db);
                        if (errorCode != 0)
                        {
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_PLOT");
                        }
                    }
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<decimal?>(tree.TREE_DBH, "TREE_DBH", "GRP_TREE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_DBH", "$V0$", "TREE_DBH", "$GRP$", "GRP_TREE");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(tree.TREE_SPP, "TREE_SPP", "GRP_TREE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_SPP", "$V0$", "TREE_SPP", "$GRP$", "GRP_TREE");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(tree.TREE_STATUS, "TREE_STATUS", "GRP_TREE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TREE_STATUS", "$V0$", "TREE_STATUS", "$GRP$", "GRP_TREE");
            }


            if (Globals.IsValidCoordinateSystem(tree.TREE_EASTWARD_DIST, tree.TREE_NORTHWARD_DIST, tree.TREE_DISTANCE_POLAR, tree.TREE_ANGLE_POLAR) > 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpTreeErrors[errorCode], "TREE_EASTWARD_DIST");
            }
            //return response;
        }

        public static void ValidateAllomResponse(GRP_ALLOM allom, IcosDbContext context, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(allom.ALLOM_DATE, "ALLOM_DATE", "GRP_ALLOM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "ALLOM_DATE", "$V0$", "ALLOM_DATE", "$GRP$", "GRP_ALLOM");
            }
            errorCode = DatesValidator.IsoDateCheck(allom.ALLOM_DATE, "ALLOM_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "ALLOM_DATE", "$V0$", "ALLOM_DATE", "$V1$", allom.ALLOM_DATE);
            }

            if (GeneralValidation.FindMandatoryNull<GRP_ALLOM>(allom, "ALLOM_DBH", "ALLOM_HEIGHT", "ALLOM_SPP", "ALLOM_STEM_BIOM", "ALLOM_BRANCHES_BIOM"))
            {
                errorCode = 1;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpAllomErrors[errorCode], "ALLOM_DBH");
            }

            //return response;
        }

        public static void ValidateAgbResponse(GRP_AGB agb, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(agb.AGB_PLOT, "AGB_PLOT", "GRP_AGB");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "AGB_PLOT", "$V0$", "AGB_PLOT", "$GRP$", "GRP_AGB");
            }
            else
            {
                if (!GeneralValidation.IsValidPlotString(agb.AGB_PLOT, agb.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "AGB_PLOT", "$V0$", "AGB_PLOT", "$V1$", agb.AGB_PLOT);
                }

                errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(agb.AGB_PLOT, agb.AGB_DATE, agb.SiteId, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "AGB_PLOT");
                }
            }

            switch (Ecosystem.ToLower())
            {
                case "cropland":
                    if (GeneralValidation.FindMandatoryNull<GRP_AGB>(agb, "AGB", "AGB_LOCATION", "AGB_AREA", "AGB_PHEN", "AGB_PTYPE"))
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.AGB_CROPLAND_MANDATORY;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB");
                    }
                    break;
                case "grassland":
                    if (GeneralValidation.FindMandatoryNull<GRP_AGB>(agb, "AGB", "AGB_AREA", "AGB_PHEN", "AGB_PLOT_TYPE"))
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.AGB_GRASSLAND_MANDATORY;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB");
                    }
                    break;
                case "forest":
                    if (String.IsNullOrEmpty(agb.AGB_PTYPE))
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_AGB_PTYPE_MANDATORY;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_PTYPE");
                    }
                    else
                    {
                        switch (agb.AGB_PTYPE.ToLower())
                        {
                            case "moss":
                                if (agb.AGB_LOCATION == null)
                                {
                                    errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_AGB_LOCATION_MANDATORY;
                                    response.Code += errorCode;
                                    response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_LOCATION");
                                }
                                if ((agb.AGB_NPP_MOSS != null && agb.AGB != null) ||
                                    (agb.AGB_NPP_MOSS == null && agb.AGB == null))
                                {
                                    errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_AGB_XOR_AGB_NPP_MOSS;
                                    response.Code += errorCode;
                                    response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_NPP_MOSS");
                                }
                                if (agb.AGB != null)
                                {
                                    if (String.IsNullOrEmpty(agb.AGB_PHEN))
                                    {
                                        errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_AGB_PHEN_MANDATORY;
                                        response.Code += errorCode;
                                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_PHEN");
                                    }
                                }
                                if (agb.AGB_NPP_MOSS != null)
                                {
                                    if (!String.IsNullOrEmpty(agb.AGB_PHEN))
                                    {
                                        errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_AGB_PHEN_MANDATORY;
                                        response.Code += errorCode;
                                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_PHEN");
                                    }
                                }
                                break;
                            case "sapling":
                                if (GeneralValidation.FindMandatoryNull<GRP_AGB>(agb, "AGB", "AGB_AREA", "AGB_LOCATION", "AGB_SPP", "AGB_PTYPE"))
                                {
                                    errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_SAPLING_MANDATORY;
                                    response.Code += errorCode;
                                    response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB");
                                }
                                break;
                            case "ferns":
                            case "herb":
                            case "shrub":
                                if (GeneralValidation.FindMandatoryNull<GRP_AGB>(agb, "AGB", "AGB_AREA", "AGB_LOCATION", "AGB_PHEN", "AGB_PTYPE"))
                                {
                                    errorCode = (int)Globals.ErrorValidationCodes.AGB_FOREST_FHS_MANDATORY;
                                    response.Code += errorCode;
                                    response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB");
                                }
                                break;
                        }
                    }
                    break;
                default:
                    if (String.IsNullOrEmpty(agb.AGB_SPP))
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.AGB_MIRES_SPP;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_SPP");
                    }
                    if ((agb.AGB_NPP_MOSS != null && agb.AGB != null) ||
                        (agb.AGB_NPP_MOSS == null && agb.AGB == null))
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.AGB_MIRES_AGB_XOR_AGB_NPP_MOSS;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_NPP_MOSS");
                    }
                    if (!String.IsNullOrEmpty(agb.AGB_ORGAN))
                    {
                        if (agb.AGB_PTYPE.ToLower() != "shrub" && agb.AGB_PTYPE.ToLower() != "sapling")
                        {
                            //return (int)Globals.ErrorValidationCodes.AGB_MIRES_AGB_ORGAN;
                            errorCode = (int)Globals.ErrorValidationCodes.AGB_MIRES_AGB_ORGAN;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpAgbErrors[errorCode], "AGB_PTYPE");
                        }
                    }
                    break;
            }
           // return response;
        }

        public static void ValidateLitterPntResponse(GRP_LITTERPNT litterPnt, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(litterPnt.LITTERPNT_PLOT, "LITTERPNT_PLOT", "GRP_LITTERPNT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LITTERPNT_PLOT", "$V0$", "LITTERPNT_PLOT", "$GRP$", "GRP_LITTERPNT");
            }
            else
            {
                if (!GeneralValidation.IsValidPlotString(litterPnt.LITTERPNT_PLOT, litterPnt.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LITTERPNT_PLOT", "$V0$", "LITTERPNT_PLOT", "$V1$", litterPnt.LITTERPNT_PLOT);
                }

                errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(litterPnt.LITTERPNT_PLOT, litterPnt.LITTERPNT_DATE, litterPnt.SiteId, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LITTERPNT_PLOT");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(litterPnt.LITTERPNT_DATE, "LITTERPNT_DATE", "GRP_LITTERPNT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LITTERPNT_DATE", "$V0$", "LITTERPNT_DATE", "$GRP$", "GRP_LITTERPNT");
            }
            errorCode = DatesValidator.IsoDateCheck(litterPnt.LITTERPNT_DATE, "LITTERPNT_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LITTERPNT_DATE", "$V0$", "LITTERPNT_DATE", "$V1$", litterPnt.LITTERPNT_DATE);
            }

            string litterType = litterPnt.LITTERPNT_TYPE.ToLower();
            switch (Ecosystem.ToLower())
            {
                case "forest":
                    if (String.Compare(litterType, "coarse") == 0)
                    {
                        if (!GeneralValidation.FindMandatoryNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT_COARSE_DIAM", "LITTERPNT_ID", "LITTERPNT_COARSE_LENGTH", "LITTERPNT_COARSE_ANGLE", "LITTERPNT_COARSE_DECAY"))
                        {
                            errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_FOREST_COARSE_MANDATORY;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT_COARSE_DIAM");
                        }
                    }
                    else if (String.Compare(litterType, "fine-woody") == 0)
                    {
                        if (!GeneralValidation.FindMandatoryNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT", "LITTERPNT_AREA"))
                        {
                            errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_FOREST_FINE_WOODY_MANDATORY;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT");
                        }
                    }
                    else if (String.Compare(litterType, "non-woody") == 0)
                    {
                        if (!GeneralValidation.FindMandatoryNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT", "LITTERPNT_AREA", "LITTERPNT_ID", "LITTERPNT_FRACTION"))
                        {
                            errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_FOREST_NON_WOODY_MANDATORY;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT");
                        }
                        if (litterPnt.LITTERPNT_FRACTION.ToLower() == "leaves")
                        {
                            if (String.IsNullOrEmpty(litterPnt.LITTERPNT_SPP))
                            {
                                errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_FOREST_SPP_MANDATORY;
                                response.Code += errorCode;
                                response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT_SPP");
                            }
                        }
                        if (GeneralValidation.IsAnyPropNotNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT_EASTWARD_DIST", "LITTERPNT_NORTHWARD_DIST", "LITTERPNT_DISTANCE_POLAR", "LITTERPNT_ANGLE_POLAR"))
                        {
                            int coo = Globals.IsValidCoordinateSystem(litterPnt.LITTERPNT_EASTWARD_DIST, litterPnt.LITTERPNT_NORTHWARD_DIST,
                                                                      litterPnt.LITTERPNT_DISTANCE_POLAR, litterPnt.LITTERPNT_ANGLE_POLAR);
                            if (coo != 0)
                            {
                                errorCode = 85;
                                response.Code += errorCode;
                                response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT_EASTWARD_DIST");
                            }
                        }
                    }
                    break;
                case "cropland":
                    if (String.Compare(litterType, "natural") == 0)
                    {
                        if (!GeneralValidation.FindMandatoryNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT", "LITTERPNT_ID", "LITTERPNT_AREA", "LITTERPNT_FRACTION"))
                        {
                            errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_CROP_NATURAL_MANDATORY;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT");
                        }
                    }
                    else if (String.Compare(litterType, "residual") == 0)
                    {
                        if (!GeneralValidation.FindMandatoryNull<GRP_LITTERPNT>(litterPnt, "LITTERPNT", "LITTERPNT_AREA", "LITTERPNT_FRACTION"))
                        {
                            errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_CROP_RESIDUAL_MANDATORY;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT");
                        }
                    }
                    else
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_LITTERTYPE_NOT_ALLOWED;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT");
                    }
                    break;
                case "grassland":
                    if (litterType != "residual" && litterType != "natural")
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_LITTERTYPE_NOT_ALLOWED;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT_TYPE");
                    }
                    if (litterPnt.LITTERPNT == null || litterPnt.LITTERPNT_AREA == null)
                    {
                        errorCode = (int)Globals.ErrorValidationCodes.LITTERPNT_AREA_GRASSLAND_MANDATORY;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpLitterPntErrors[errorCode], "LITTERPNT_TYPE");
                    }

                    break;
            }
            //return response;
        }
    }
}
