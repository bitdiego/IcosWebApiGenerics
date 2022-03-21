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
        private static Response response = null;
        private static string Err = "";
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static Response GetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            return response;
        }

        //OK
        public static Response ValidateLocationResponse(GRP_LOCATION location)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(location.LOCATION_DATE, "LOCATION_DATE", "GRP_LOCATION");
            if (errorCode != 0)
            {
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOCATION_DATE", "$V0$", "LOCATION_DATE", "$GRP$", "GRP_LOCATION");
            }
            errorCode = DatesValidator.IsoDateCheck(location.LOCATION_DATE, "LOCATION_DATE");
            if (errorCode != 0)
            {
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOCATION_DATE", "$V0$", "LOCATION_DATE", "$V1$", location.LOCATION_DATE);
            }
            
            if (location.LOCATION_LAT > Globals.MAX_LATITUDE_VALUE || location.LOCATION_LAT < -Globals.MAX_LATITUDE_VALUE)
            {
                errorCode = 6;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LAT", "$V0$", location.LOCATION_LAT.ToString());
            }
            if (location.LOCATION_LONG > Globals.MAX_LONGITUDE_VALUE|| location.LOCATION_LONG < -Globals.MAX_LONGITUDE_VALUE)
            {
                errorCode = 5;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LONG", "$V0$", location.LOCATION_LONG.ToString());
            }
            return response;
        }

        //OK
        public static Response ValidateUtcResponse(GRP_UTC_OFFSET utc)
        {
            errorCode = DatesValidator.IsoDateCheck(utc.UTC_OFFSET_DATE_START, "UTC_OFFSET_DATE_START");
            if (errorCode != 0)
            {
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "UTC_OFFSET_DATE_START", "$V0$", "UTC_OFFSET_DATE_START", "$V1$", utc.UTC_OFFSET_DATE_START);
            }
            if (Decimal.Compare(utc.UTC_OFFSET, Globals.MIN_UTC_OFFSET_VAL) < 0 || Decimal.Compare(utc.UTC_OFFSET, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                errorCode = 1;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpUtcErrors[errorCode], "UTC_OFFSET", "$V0$", utc.UTC_OFFSET.ToString());
            }
            return response;
        }

        //OK
        public static async Task<Response> ValidateLandOwnerResponseAsync(GRP_LAND_OWNERSHIP land, IcosDbContext db)
        {
            if (!String.IsNullOrEmpty(land.LAND_DATE))
            {
                errorCode = DatesValidator.IsoDateCheck(land.LAND_DATE, "LAND_DATE");
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_DATE", "$V0$", "LAND_DATE", "$V1$", land.LAND_DATE);
                }
            }

            if (!String.IsNullOrEmpty(land.LAND_OWNERSHIP))
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(land.LAND_OWNERSHIP, (int)Globals.CvIndexes.LAND_OWNERSHIP, db);
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_OWNERSHIP", "$V0$", land.LAND_OWNERSHIP, "$V1$", "LAND_OWNERSHIP", "$GRP$", "GRP_LAND_OWNERSHIP");
            }
            return response;
        }

        
        public static async Task<Response> ValidateTowerResponseAsync(GRP_TOWER tower, IcosDbContext db)
        {
            GeneralValidation.MissingMandatoryData<string>(tower.TOWER_DATE, "TOWER_DATE", "GRP_TOWER");
            DatesValidator.IsoDateCheck( tower.TOWER_DATE, "TOWER_DATE");
            await GeneralValidation.ItemInBadmListAsync( tower.TOWER_TYPE, (int)Globals.CvIndexes.TOWER_TYPE, db);
            await GeneralValidation.ItemInBadmListAsync( tower.TOWER_ACCESS,  (int)Globals.CvIndexes.TOWER_ACCESS, db);
            await GeneralValidation.ItemInBadmListAsync( tower.TOWER_POWER, (int)Globals.CvIndexes.TOWER_POWER, db);
            await GeneralValidation.ItemInBadmListAsync( tower.TOWER_DATATRAN,  (int)Globals.CvIndexes.TOWER_DATATRAN, db);
            return response;
        }

        
        //to do
        public static Response ValidateClimateAvgResponse(GRP_CLIM_AVG climateAvg)
        {
            //MissingDate(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
            GeneralValidation.MissingMandatoryData<string>(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
            DatesValidator.IsoDateCheck(climateAvg.MAC_DATE, "MAC_DATE");
            //check if MAP, MAR, MAS, MAC_YEARS must only be positive
            return response;
        }

        //to do
        public static Response ValidateDistManResponse(GRP_DM distMan, IcosDbContext db)
        {
            DatesValidator.IsoDateCheck(distMan.DM_DATE, "DM_DATE");
            DatesValidator.IsoDateCheck(distMan.DM_DATE_START, "DM_DATE_START");
            DatesValidator.IsoDateCheck(distMan.DM_DATE_END, "DM_DATE_END");
            if(!String.IsNullOrEmpty(distMan.DM_DATE)&& !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                errorCode = 4;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                response.FormatError("DM_DATE", "$V0$", distMan.DM_DATE, "$V1$", distMan.DM_DATE_START, "$V2$", distMan.DM_DATE_END);
                //Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE, "$V1$", distMan.DM_DATE_START, "$V2$", distMan.DM_DATE_END);
            }
            if ( !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                if(String.Compare(distMan.DM_DATE_START, distMan.DM_DATE_END) > 0)
                {
                    errorCode = 6;
                    response.Code += errorCode;
                    Err = ErrorCodes.GeneralErrors[errorCode];
                    response.FormatError("DM_DATE_START", "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                    //Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                }
            }
            /*
            GeneralValidation.ItemInBadmList("DM_ENCROACH", distMan.DM_ENCROACH, "GRP_DM", (int)Globals.CvIndexes.DM_ENCROACH, db);
            GeneralValidation.ItemInBadmList("DM_AGRICULTURE", distMan.DM_AGRICULTURE, "GRP_DM", (int)Globals.CvIndexes.DM_AGRICULTURE, db);
            GeneralValidation.ItemInBadmList("DM_EXT_WEATHER", distMan.DM_EXT_WEATHER, "GRP_DM", (int)Globals.CvIndexes.DM_EXT_WEATHER, db);
            GeneralValidation.ItemInBadmList("DM_FERT_M", distMan.DM_FERT_M, "GRP_DM", (int)Globals.CvIndexes.DM_FERT_M, db);
            GeneralValidation.ItemInBadmList("DM_FERT_O", distMan.DM_FERT_M, "DM_FERT_O", (int)Globals.CvIndexes.DM_FERT_O, db); 
            GeneralValidation.ItemInBadmList("DM_FIRE", distMan.DM_FERT_M, "DM_FIRE", (int)Globals.CvIndexes.DM_FIRE, db);
            GeneralValidation.ItemInBadmList("DM_FORESTRY", distMan.DM_FERT_M, "DM_FORESTRY", (int)Globals.CvIndexes.DM_FORESTRY, db);
            GeneralValidation.ItemInBadmList("DM_GRAZE", distMan.DM_FERT_M, "DM_GRAZE", (int)Globals.CvIndexes.DM_GRAZE, db);
            GeneralValidation.ItemInBadmList("DM_INS_PATH", distMan.DM_FERT_M, "DM_INS_PATH", (int)Globals.CvIndexes.DM_INS_PATH, db);
            GeneralValidation.ItemInBadmList("DM_PESTICIDE", distMan.DM_FERT_M, "DM_PESTICIDE", (int)Globals.CvIndexes.DM_PESTICIDE, db);
            GeneralValidation.ItemInBadmList("DM_PLANTING", distMan.DM_FERT_M, "DM_PLANTING", (int)Globals.CvIndexes.DM_PLANTING, db);
            GeneralValidation.ItemInBadmList("DM_TILL", distMan.DM_FERT_M, "DM_TILL", (int)Globals.CvIndexes.DM_TILL, db); 
            GeneralValidation.ItemInBadmList("DM_WATER", distMan.DM_FERT_M, "DM_WATER", (int)Globals.CvIndexes.DM_WATER, db);
            GeneralValidation.ItemInBadmList("DM_GENERAL", distMan.DM_FERT_M, "DM_GENERAL", (int)Globals.CvIndexes.DM_GENERAL, db);
            */
            if (!GeneralValidation.IsAnyPropNotNull<GRP_DM>(distMan))
            {
                errorCode = 9;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
            }

            return response;
        }


        //seems OK
        public static async Task<Response> ValidateSamplingSchemeResponseAsync(GRP_PLOT samplingScheme, IcosDbContext db)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(samplingScheme.PLOT_DATE, "PLOT_DATE", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_DATE", "$V0$", "PLOT_DATE", "$GRP$", "GRP_PLOT");
            }
            errorCode = DatesValidator.IsoDateCheck(samplingScheme.PLOT_DATE, "PLOT_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_DATE", "$V0$", "PLOT_DATE", "$V1$", samplingScheme.PLOT_DATE);
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(samplingScheme.PLOT_ID, "PLOT_ID", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_ID", "$V0$", "PLOT_ID", "$GRP$", "GRP_PLOT");
            }
            else
            {
                if(!GeneralValidation.IsValidPlotString(samplingScheme.PLOT_ID, samplingScheme.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_ID", "$V0$", "PLOT_ID", "$V1$", samplingScheme.PLOT_ID);
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(samplingScheme.PLOT_TYPE, "PLOT_TYPE", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_TYPE", "$V0$", "PLOT_TYPE", "$GRP$", "GRP_PLOT");
            }
            else
            {
                //check if plot_type in controlled vocabulary
                errorCode = await GeneralValidation.ItemInBadmListAsync( samplingScheme.PLOT_TYPE, (int)Globals.CvIndexes.PLOTTYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_TYPE", "$V0$", samplingScheme.PLOT_TYPE, "$V1$", "PLOTTYPE", "$GRP$", "GRP_PLOT");
                }
                string _subPlot = samplingScheme.PLOT_ID.Substring(0, samplingScheme.PLOT_ID.IndexOf('_'));
                errorCode = 1;
                response.Code += errorCode;
                if (String.Compare(_subPlot, samplingScheme.PLOT_TYPE, true) != 0)
                {
                    response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_TYPE", "$V0$", _subPlot, "$V1$", samplingScheme.PLOT_TYPE);
                }
            }

            //validate coordinate system:  PLOT_EASTWARD_DIST/PLOT_NORTHWARD_DIST or PLOT_ANGLE_POLAR/PLOT_DISTANCE_POLAR or PLOT_LOCATION_LAT/PLOT_LOCATION_LONG
            //must be mutually exclusive
            int xc = Globals.IsValidCoordinateSystem<decimal?>(samplingScheme.PLOT_EASTWARD_DIST, samplingScheme.PLOT_NORTHWARD_DIST,
                                                               samplingScheme.PLOT_DISTANCE_POLAR, samplingScheme.PLOT_ANGLE_POLAR,
                                                               samplingScheme.PLOT_LOCATION_LAT, samplingScheme.PLOT_LOCATION_LONG);
            if (xc > 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_EASTWARD_DIST");
            }

            if (!String.IsNullOrEmpty(samplingScheme.PLOT_REFERENCE_POINT))
            {
                //check if plot_type in controlled vocabulary
                errorCode = await GeneralValidation.ItemInBadmListAsync(samplingScheme.PLOT_REFERENCE_POINT, (int)Globals.CvIndexes.PLOTREF, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_REFERENCE_POINT", "$V0$", samplingScheme.PLOT_REFERENCE_POINT, "$V1$", "PLOTREF", "$GRP$", "GRP_PLOT");
                }
                if (!samplingScheme.PLOT_ID.ToLower().StartsWith("sp-ii"))
                {
                    errorCode = 3;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                }
                if (samplingScheme.PLOT_LOCATION_LAT != null && samplingScheme.PLOT_LOCATION_LONG != null)
                {
                    errorCode = 4;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                }
            }
            else
            {
                if (samplingScheme.PLOT_ID.ToLower().StartsWith("sp-ii"))
                {
                    if ((samplingScheme.PLOT_ANGLE_POLAR != null && samplingScheme.PLOT_DISTANCE_POLAR != null) ||
                    (samplingScheme.PLOT_EASTWARD_DIST != null && samplingScheme.PLOT_NORTHWARD_DIST != null))
                    {
                        errorCode = 5;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                    }
                }
            }
            return response;
        }

        //validate also numeric values...
        public static async Task<Response> ValidateFlsmResponseAsync(GRP_FLSM flsm, IcosDbContext db)
        {
            //to do::: FLSM_PLOT_ID present in GRP_PLOT
            errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(flsm.FLSM_PLOT_ID, flsm.FLSM_DATE, flsm.SiteId, db);
            if (errorCode > 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_PLOT_ID");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(flsm.FLSM_DATE, "FLSM_DATE", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_DATE", "$V0$", "FLSM_DATE", "$GRP$", "GRP_FLSM");
            }
            errorCode = DatesValidator.IsoDateCheck(flsm.FLSM_DATE, "FLSM_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_DATE", "$V0$", "FLSM_DATE", "$V1$", flsm.FLSM_DATE);
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(flsm.FLSM_SAMPLE_TYPE, "FLSM_SAMPLE_TYPE", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_TYPE", "$V0$", "FLSM_SAMPLE_TYPE", "$GRP$", "GRP_FLSM");
            }
            else 
            {
                errorCode= await GeneralValidation.ItemInBadmListAsync(flsm.FLSM_SAMPLE_TYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_TYPE", "$V0$", flsm.FLSM_SAMPLE_TYPE, "$V1$", "FLSM_STYPE", "$GRP$", "GRP_FLSM");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<int>(flsm.FLSM_SAMPLE_ID, "FLSM_SAMPLE_ID", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_ID", "$V0$", "FLSM_SAMPLE_ID", "$GRP$", "GRP_FLSM");
            }

            if (!GeneralValidation.XORNull<string>(flsm.FLSM_SPP, flsm.FLSM_PTYPE))
            {
                errorCode = 1;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpFlsmErrors[errorCode], "FLSM_SPP");
            }

            if (!String.IsNullOrEmpty(flsm.FLSM_PTYPE))
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(flsm.FLSM_PTYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_PTYPE", "$V0$", flsm.FLSM_PTYPE, "$V1$", "FLSM_PTYPE", "$GRP$", "GRP_FLSM");
                }
            }

            return response;
        }

        public static Response ValidateSosmResponse(GRP_SOSM sosm, IcosDbContext db)
        {
            //to do::: SOSM_PLOT_ID present in GRP_PLOT
            errorCode = GeneralValidation.ItemInSamplingPointGroupAsync(sosm.SOSM_PLOT_ID, sosm.SOSM_DATE, sosm.SiteId, db);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SOSM_PLOT_ID");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(sosm.SOSM_DATE, "SOSM_DATE", "GRP_SOSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SOSM_DATE", "$V0$", "SOSM_DATE", "$GRP$", "GRP_SOSM");
            }
            errorCode = DatesValidator.IsoDateCheck(sosm.SOSM_DATE, "SOSM_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SOSM_DATE", "$V0$", "SOSM_DATE", "$V1$", sosm.SOSM_DATE);
            }

            //
            string sosmPlotid = sosm.SOSM_PLOT_ID.ToLower();
            string sosmSampleMat = sosm.SOSM_SAMPLE_MAT.ToLower();
            if (sosmPlotid.StartsWith("sp-i") && !sosmPlotid.StartsWith("sp-ii"))
            {
                if (sosmSampleMat.StartsWith("o"))
                {
                    errorCode = 2;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_PLOT_ID");
                }
                if (sosm.SOSM_UD == null || sosm.SOSM_LD == null)
                {
                    errorCode = 3;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_UD");
                }
                if (!GeneralValidation.IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiSosmM))
                {
                    errorCode = 7;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                }
            }
            else if (sosmPlotid.StartsWith("sp-ii"))
            {
                if (sosmSampleMat == "m")
                {
                    if (sosm.SOSM_UD == null || sosm.SOSM_LD == null)
                    {
                        errorCode = 3;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_UD");
                    }
                    if (!GeneralValidation.CountBoundedProps<GRP_SOSM>(sosm, 2, "SOSM_AREA", "SOSM_VOLUME"))
                    {
                        errorCode = 2;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_UD");
                    }
                    if (!GeneralValidation.IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiiSosmM))
                    {
                        errorCode = 8;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                    }
                }
                else
                {
                    //O, Oi, Oa, Oe
                    if (!GeneralValidation.CountBoundedProps<GRP_SOSM>(sosm, 3, "SOSM_THICKNESS", "SOSM_AREA", "SOSM_VOLUME"))
                    {
                        errorCode = 5;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                    }
                    if (!GeneralValidation.IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiiSosmOrganic))
                    {
                        errorCode = 9;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                    }
                }

            }
            else
            {
                errorCode = 6;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_PLOT_ID");
            }
            return response;
        }

        public static async Task<Response> ValidateDhpResponseAsync(GRP_DHP dhp, IcosDbContext db)
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

            return response;
        }

        public static async Task<Response> ValidateGaiResponseAsync(GRP_GAI gai, IcosDbContext db)
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
                if(!GeneralValidation.IsValidPlotString(gai.GAI_PLOT, gai.GroupId))
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
                errorCode = await GeneralValidation.ItemInBadmListAsync(gai.GAI_METHOD, (int)Globals.CvIndexes.GAIMETHOD, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_METHOD", "$V0$", gai.GAI_METHOD, "$V1$", "GAI_METHOD", "$GRP$", "GRP_GAI");
                }
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
            return response;
        }

        public static Response ValidateCeptResponse(GRP_CEPT cept)
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
            return response;
        }


        public static async Task<Response> ValidateBulkhResponseAsync(GRP_BULKH bulkh, IcosDbContext db)
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
                errorCode = await GeneralValidation.ItemInBadmListAsync(bulkh.BULKH_PLOT_TYPE, (int)Globals.CvIndexes.PLOTTYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH_PLOT_TYPE", "$V0$", bulkh.BULKH_PLOT_TYPE, "$V1$", "BULKH_PLOT_TYPE", "$GRP$", "GRP_BULKH");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<decimal>(bulkh.BULKH, "BULKH", "GRP_BULKH");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BULKH", "$V0$", "BULKH", "$GRP$", "GRP_BULKH");
            }
            return response;
        }

        public static async Task<Response> ValidateSppsResponseAsync(GRP_SPPS spps, IcosDbContext db)
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
                errorCode = await GeneralValidation.ItemInBadmListAsync(spps.SPPS_TWSP_PCT, (int)Globals.CvIndexes.TWSP, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_TWSP_PCT", "$V0$", spps.SPPS_TWSP_PCT, "$V1$", "SPPS_TWSP_PCT", "$GRP$", "GRP_SPPS");
                }
            }

            if (!String.IsNullOrEmpty(spps.SPPS_PTYPE))
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(spps.SPPS_PTYPE, (int)Globals.CvIndexes.SPPPTYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SPPS_PTYPE", "$V0$", spps.SPPS_PTYPE, "$V1$", "SPPS_PTYPE", "$GRP$", "GRP_SPPS");
                }
            }

            return response;
        }

        public static Response ValidateTreeResponse(GRP_TREE tree, IcosDbContext db)
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
            return response;
        }

        public static Response ValidateAgbResponse(GRP_AGB agb, IcosDbContext db)
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
                        errorCode =  (int)Globals.ErrorValidationCodes.AGB_CROPLAND_MANDATORY;
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
            return response;
        }

        public static Response ValidateLitterPntResponse(GRP_LITTERPNT litterPnt, IcosDbContext db)
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
            return response;
        }

        public static Response ValidateAllomResponse(GRP_ALLOM allom, IcosDbContext context)
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

            return response;
        }

        public static Response ValidateDSnowResponse(GRP_D_SNOW dSnow, IcosDbContext db)
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
            return response;
        }

        public static Response ValidateWtdPntResponse(GRP_WTDPNT wtdPnt, IcosDbContext db)
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

            return response;
        }

        public static async Task<Response> ValidateInstResponseAsync(GRP_INST inst, IcosDbContext db)
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
            return response;
        }
        public static async Task<Response> ValidateLoggerResponseAsync(GRP_LOGGER logger, IcosDbContext db)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_MODEL, "LOGGER_MODEL", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_MODEL", "$V0$", "LOGGER_MODEL", "$GRP$", "GRP_LOGGER");
            }
            else
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(logger.LOGGER_MODEL, (int)Globals.CvIndexes.LOGGER_MODEL, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_MODEL", "$V0$", logger.LOGGER_MODEL, "$V1$", "LOGGER_MODEL", "$GRP$", "GRP_LOGGER");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_SN, "LOGGER_SN", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_SN", "$V0$", "LOGGER_SN", "$GRP$", "GRP_LOGGER");
            }
            else
            {
                errorCode = InstrumentsValidation.SerialNumberCheck(logger.LOGGER_MODEL, logger.LOGGER_SN);
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_SN", "$V0$", "LOGGER_SN", "$GRP$", "GRP_LOGGER");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<int>(logger.LOGGER_ID, "LOGGER_ID", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_ID", "$V0$", "LOGGER_ID", "$GRP$", "GRP_LOGGER");
            }
            
            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_DATE, "LOGGER_DATE", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_DATE", "$V0$", "LOGGER_DATE", "$GRP$", "GRP_LOGGER");
            }
            errorCode = DatesValidator.IsoDateCheck(logger.LOGGER_DATE, "LOGGER_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_DATE", "$V0$", "LOGGER_DATE", "$V1$", logger.LOGGER_DATE);
            }



            return response;
        }

        public static async Task<Response> ValidateEcResponseAsync(GRP_EC ecInst, IcosDbContext db)
        {

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_MODEL, "EC_MODEL", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_MODEL", "$V0$", "EC_MODEL", "$GRP$", "GRP_EC");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_SN, "EC_SN", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_SN", "$V0$", "EC_SN", "$GRP$", "GRP_EC");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_TYPE, "EC_TYPE", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_TYPE", "$V0$", "EC_TYPE", "$GRP$", "GRP_EC");
            }

            //ec sensor present in GRP_INST
            if(!String.IsNullOrEmpty(ecInst.EC_DATE) || !String.IsNullOrEmpty(ecInst.EC_DATE_START))
            {
                string dateToCheck = String.IsNullOrEmpty(ecInst.EC_DATE) ? ecInst.EC_DATE_START : ecInst.EC_DATE;
                errorCode = await InstrumentsValidation.SensorInGrpInst(ecInst.EC_MODEL, ecInst.EC_SN, dateToCheck, ecInst.SiteId, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "EC_MODEL");
                }
            }

            //check dates constraints
            errorCode = DatesValidator.IsoDateCompare(ecInst.EC_DATE, ecInst.EC_DATE_START, ecInst.EC_DATE_END);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_DATE", "$V0$", "EC_DATE", "$V1$", "EC_DATE_START", "$V2$", "EC_DATE_END", "$GRP$", "GRP_EC");
            }

            return response;
        }

        /////////////////////////////////
        
        private static int ValidateGaiByMethod(GRP_GAI model,string ecosystem)
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
