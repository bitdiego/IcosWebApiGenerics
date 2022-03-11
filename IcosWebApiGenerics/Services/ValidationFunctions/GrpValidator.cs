﻿using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using System.Reflection;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public class GrpValidator
    {
        private static Response response = null;
        private static string Err = "";
        private static int errorCode = 0;

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
            errorCode =  MissingMandatoryData<string>(location.LOCATION_DATE, "LOCATION_DATE", "GRP_LOCATION");
            if (errorCode != 0)
            {
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOCATION_DATE", "$V0$", "LOCATION_DATE", "$GRP$", "GRP_LOCATION");
            }
            errorCode = IsoDateCheck(location.LOCATION_DATE, "LOCATION_DATE");
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
            errorCode = IsoDateCheck(utc.UTC_OFFSET_DATE_START, "UTC_OFFSET_DATE_START");
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
        public static Response ValidateLandOwnerResponse(GRP_LAND_OWNERSHIP land, IcosDbContext db)
        {
            if (!String.IsNullOrEmpty(land.LAND_DATE))
            {
                errorCode = IsoDateCheck(land.LAND_DATE, "LAND_DATE");
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_DATE", "$V0$", "LAND_DATE", "$V1$", land.LAND_DATE);
                }
            }

            if (!String.IsNullOrEmpty(land.LAND_OWNERSHIP))
            {
                errorCode = ItemInBadmList(land.LAND_OWNERSHIP, (int)Globals.CvIndexes.LAND_OWNERSHIP, db);
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_OWNERSHIP", "$V0$", land.LAND_OWNERSHIP, "$V1$", "LAND_OWNERSHIP", "$GRP$", "GRP_LAND_OWNERSHIP");
            }
            return response;
        }

        
        public static Response ValidateTowerResponse(GRP_TOWER tower, IcosDbContext db)
        {
            MissingMandatoryData<string>(tower.TOWER_DATE, "TOWER_DATE", "GRP_TOWER");
            IsoDateCheck( tower.TOWER_DATE, "TOWER_DATE");
            ItemInBadmList( tower.TOWER_TYPE, (int)Globals.CvIndexes.TOWER_TYPE, db);
            ItemInBadmList( tower.TOWER_ACCESS,  (int)Globals.CvIndexes.TOWER_ACCESS, db);
            ItemInBadmList( tower.TOWER_POWER, (int)Globals.CvIndexes.TOWER_POWER, db);
            ItemInBadmList( tower.TOWER_DATATRAN,  (int)Globals.CvIndexes.TOWER_DATATRAN, db);
            return response;
        }

        //to do
        public static Response ValidateClimateAvgResponse(GRP_CLIM_AVG climateAvg)
        {
            //MissingDate(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
            MissingMandatoryData<string>(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
            IsoDateCheck(climateAvg.MAC_DATE, "MAC_DATE");
            //check if MAP, MAR, MAS, MAC_YEARS must only be positive
            return response;
        }

        //to do
        public static Response ValidateDistManResponse(GRP_DM distMan, IcosDbContext db)
        {
            IsoDateCheck(distMan.DM_DATE, "DM_DATE");
            IsoDateCheck(distMan.DM_DATE_START, "DM_DATE_START");
            IsoDateCheck(distMan.DM_DATE_END, "DM_DATE_END");
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
            ItemInBadmList("DM_ENCROACH", distMan.DM_ENCROACH, "GRP_DM", (int)Globals.CvIndexes.DM_ENCROACH, db);
            ItemInBadmList("DM_AGRICULTURE", distMan.DM_AGRICULTURE, "GRP_DM", (int)Globals.CvIndexes.DM_AGRICULTURE, db);
            ItemInBadmList("DM_EXT_WEATHER", distMan.DM_EXT_WEATHER, "GRP_DM", (int)Globals.CvIndexes.DM_EXT_WEATHER, db);
            ItemInBadmList("DM_FERT_M", distMan.DM_FERT_M, "GRP_DM", (int)Globals.CvIndexes.DM_FERT_M, db);
            ItemInBadmList("DM_FERT_O", distMan.DM_FERT_M, "DM_FERT_O", (int)Globals.CvIndexes.DM_FERT_O, db); 
            ItemInBadmList("DM_FIRE", distMan.DM_FERT_M, "DM_FIRE", (int)Globals.CvIndexes.DM_FIRE, db);
            ItemInBadmList("DM_FORESTRY", distMan.DM_FERT_M, "DM_FORESTRY", (int)Globals.CvIndexes.DM_FORESTRY, db);
            ItemInBadmList("DM_GRAZE", distMan.DM_FERT_M, "DM_GRAZE", (int)Globals.CvIndexes.DM_GRAZE, db);
            ItemInBadmList("DM_INS_PATH", distMan.DM_FERT_M, "DM_INS_PATH", (int)Globals.CvIndexes.DM_INS_PATH, db);
            ItemInBadmList("DM_PESTICIDE", distMan.DM_FERT_M, "DM_PESTICIDE", (int)Globals.CvIndexes.DM_PESTICIDE, db);
            ItemInBadmList("DM_PLANTING", distMan.DM_FERT_M, "DM_PLANTING", (int)Globals.CvIndexes.DM_PLANTING, db);
            ItemInBadmList("DM_TILL", distMan.DM_FERT_M, "DM_TILL", (int)Globals.CvIndexes.DM_TILL, db); 
            ItemInBadmList("DM_WATER", distMan.DM_FERT_M, "DM_WATER", (int)Globals.CvIndexes.DM_WATER, db);
            ItemInBadmList("DM_GENERAL", distMan.DM_FERT_M, "DM_GENERAL", (int)Globals.CvIndexes.DM_GENERAL, db);
            */
            if (!IsAnyPropNotNull<GRP_DM>(distMan))
            {
                errorCode = 9;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
            }

            return response;
        }

        //seems OK
        public static Response ValidateSamplingSchemeResponse(GRP_PLOT samplingScheme, IcosDbContext db)
        {
            errorCode = MissingMandatoryData<string>(samplingScheme.PLOT_DATE, "PLOT_DATE", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_DATE", "$V0$", "PLOT_DATE", "$GRP$", "GRP_PLOT");
            }
            errorCode = IsoDateCheck(samplingScheme.PLOT_DATE, "PLOT_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_DATE", "$V0$", "PLOT_DATE", "$V1$", samplingScheme.PLOT_DATE);
            }
            errorCode = MissingMandatoryData<string>(samplingScheme.PLOT_ID, "PLOT_ID", "GRP_PLOT");
            
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_ID", "$V0$", "PLOT_ID", "$GRP$", "GRP_PLOT");
            }

            errorCode = MissingMandatoryData<string>(samplingScheme.PLOT_TYPE, "PLOT_TYPE", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_TYPE", "$V0$", "PLOT_TYPE", "$GRP$", "GRP_PLOT");
            }
            else
            {
                //check if plot_type in controlled vocabulary
                errorCode = ItemInBadmList( samplingScheme.PLOT_TYPE, (int)Globals.CvIndexes.PLOTTYPE, db);
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
                errorCode = ItemInBadmList(samplingScheme.PLOT_REFERENCE_POINT, (int)Globals.CvIndexes.PLOTREF, db);
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
        public static Response ValidateFlsmResponse(GRP_FLSM flsm, IcosDbContext db)
        {
            //to do::: FLSM_PLOT_ID present in GRP_PLOT
            errorCode = MissingMandatoryData<string>(flsm.FLSM_DATE, "FLSM_DATE", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_DATE", "$V0$", "FLSM_DATE", "$GRP$", "GRP_FLSM");
            }
            errorCode = IsoDateCheck(flsm.FLSM_DATE, "FLSM_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_DATE", "$V0$", "FLSM_DATE", "$V1$", flsm.FLSM_DATE);
            }

            errorCode = MissingMandatoryData<string>(flsm.FLSM_SAMPLE_TYPE, "FLSM_SAMPLE_TYPE", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_TYPE", "$V0$", "FLSM_SAMPLE_TYPE", "$GRP$", "GRP_FLSM");
            }
            else 
            {
                errorCode=ItemInBadmList(flsm.FLSM_SAMPLE_TYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_TYPE", "$V0$", flsm.FLSM_SAMPLE_TYPE, "$V1$", "FLSM_STYPE", "$GRP$", "GRP_FLSM");
                }
            }

            errorCode = MissingMandatoryData<int>(flsm.FLSM_SAMPLE_ID, "FLSM_SAMPLE_ID", "GRP_FLSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_ID", "$V0$", "FLSM_SAMPLE_ID", "$GRP$", "GRP_FLSM");
            }

            if (!XORNull<string>(flsm.FLSM_SPP, flsm.FLSM_PTYPE))
            {
                errorCode = 1;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpFlsmErrors[errorCode], "FLSM_SPP");
            }

            if (!String.IsNullOrEmpty(flsm.FLSM_PTYPE))
            {
                errorCode = ItemInBadmList(flsm.FLSM_PTYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_PTYPE", "$V0$", flsm.FLSM_PTYPE, "$V1$", "FLSM_PTYPE", "$GRP$", "GRP_FLSM");
                }
            }

            return response;
        }

        public static Response ValidateSosmResponse(GRP_SOSM sosm, IcosDbContext context)
        {
            //to do::: SOSM_PLOT_ID present in GRP_PLOT
            errorCode = MissingMandatoryData<string>(sosm.SOSM_DATE, "SOSM_DATE", "GRP_SOSM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "SOSM_DATE", "$V0$", "SOSM_DATE", "$GRP$", "GRP_SOSM");
            }
            errorCode = IsoDateCheck(sosm.SOSM_DATE, "SOSM_DATE");
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
                if (!IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiSosmM))
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
                    if (!CountBoundedProps<GRP_SOSM>(sosm, 2, "SOSM_AREA", "SOSM_VOLUME"))
                    {
                        errorCode = 2;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_UD");
                    }
                    if (!IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiiSosmM))
                    {
                        errorCode = 8;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                    }
                }
                else
                {
                    //O, Oi, Oa, Oe
                    if (!CountBoundedProps<GRP_SOSM>(sosm, 3, "SOSM_THICKNESS", "SOSM_AREA", "SOSM_VOLUME"))
                    {
                        errorCode = 5;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSosmErrors[errorCode], "SOSM_SAMPLE_ID");
                    }
                    if (!IsValidPattern(sosm.SOSM_SAMPLE_ID, Globals.spiiSosmOrganic))
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

        public static Response ValidateDhpResponse(GRP_DHP dhp, IcosDbContext db)
        {
            errorCode = MissingMandatoryData<int>(dhp.DHP_ID, "DHP_ID", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_ID", "$V0$", "DHP_ID", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be integer value...
            }

            errorCode = MissingMandatoryData<string>(dhp.DHP_CAMERA, "DHP_CAMERA", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA", "$V0$", "DHP_CAMERA", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be in badmlist
                errorCode = ItemInBadmList(dhp.DHP_CAMERA, (int)Globals.CvIndexes.CAMERA, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA", "$V0$", dhp.DHP_CAMERA, "$V1$", "CAMERA", "$GRP$", "GRP_DHP");
                }
            }

            errorCode = MissingMandatoryData<string>(dhp.DHP_CAMERA_SN, "DHP_CAMERA_SN", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_CAMERA_SN", "$V0$", "DHP_CAMERA_SN", "$GRP$", "GRP_DHP");
            }

            errorCode = MissingMandatoryData<string>(dhp.DHP_LENS, "DHP_LENS", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS", "$V0$", "DHP_LENS", "$GRP$", "GRP_DHP");
            }
            else
            {
                //must be in badmlist
                errorCode = ItemInBadmList(dhp.DHP_LENS, (int)Globals.CvIndexes.LENS, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS", "$V0$", dhp.DHP_LENS, "$V1$", "LENS", "$GRP$", "GRP_DHP");
                }
            }

            errorCode = MissingMandatoryData<string>(dhp.DHP_LENS_SN, "DHP_LENS_SN", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_LENS_SN", "$V0$", "DHP_LENS_SN", "$GRP$", "GRP_DHP");
            }

            bool isRowCol = true;
            errorCode = MissingMandatoryData<int>(dhp.DHP_OC_ROW, "DHP_OC_ROW", "GRP_DHP");
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

            errorCode = MissingMandatoryData<int>(dhp.DHP_OC_COL, "DHP_OC_COL", "GRP_DHP");
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

            errorCode = MissingMandatoryData<decimal>(dhp.DHP_RADIUS, "DHP_RADIUS", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_RADIUS", "$V0$", "DHP_RADIUS", "$GRP$", "GRP_DHP");
            }

            errorCode = MissingMandatoryData<string>(dhp.DHP_DATE, "DHP_DATE", "GRP_DHP");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_DATE", "$V0$", "DHP_DATE", "$GRP$", "GRP_DHP");
            }
            errorCode = IsoDateCheck(dhp.DHP_DATE, "DHP_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DHP_DATE", "$V0$", "DHP_DATE", "$V1$", dhp.DHP_DATE);
            }

            return response;
        }

        public static Response ValidateGaiResponse(GRP_GAI gai, IcosDbContext db)
        {
            errorCode = MissingMandatoryData<string>(gai.GAI_PLOT, "GAI_PLOT", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_PLOT", "$V0$", "GAI_PLOT", "$GRP$", "GRP_GAI");
            }
            else
            {
                //validation of plot reg ex
                if(!IsValidPlotString(gai.GAI_PLOT, gai.GroupId))
                {
                    errorCode = 1;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpGaiErrors[errorCode], "GAI_PLOT", "$V0$", gai.GAI_PLOT);
                }
            }

            errorCode = MissingMandatoryData<string>(gai.GAI_METHOD, "GAI_METHOD", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_METHOD", "$V0$", "GAI_METHOD", "$GRP$", "GRP_GAI");
            }
            else
            {
                //in badm list
                errorCode = ItemInBadmList(gai.GAI_METHOD, (int)Globals.CvIndexes.GAIMETHOD, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_METHOD", "$V0$", gai.GAI_METHOD, "$V1$", "GAI_METHOD", "$GRP$", "GRP_GAI");
                }
            }

            errorCode = MissingMandatoryData<string>(gai.GAI_DATE, "GAI_DATE", "GRP_GAI");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_DATE", "$V0$", "GAI_DATE", "$GRP$", "GRP_GAI");
            }
            errorCode = IsoDateCheck(gai.GAI_DATE, "GAI_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "GAI_DATE", "$V0$", "GAI_DATE", "$V1$", gai.GAI_DATE);
            }

            string ecosystem = ""; //get by site id!!!
            errorCode = ValidateGaiByMethod(gai, ecosystem);
            return response;
        }

        /////////////////////////////////
        ///

        private static int ItemInBadmList(string value, int cvIndex, IcosDbContext db)
        {
            string bmList = db.BADMList.Where(item => item.cv_index == cvIndex).Select(x => x.vocabulary).FirstOrDefault();
            var res = db.BADMList.Where(item => item.cv_index == cvIndex)
                                 .Any(item => (String.Compare(item.shortname, value, true) == 0));
            if (!res)
            {
                return 7;
            }
            return 0;
        }

        public static int MissingMandatoryData<T>(T value, string name, string groupName)
        {
            if (value == null)
            {
               return 1;
            }
            return 0;
        }

        private static int IsoDateCheck(string dateValue, string name)
        {
            if (String.IsNullOrEmpty(dateValue))
            {
                return 0;
            }
            errorCode = ValidateIsoDate(dateValue);
            if (errorCode > 0)
            {
                return 2;
            }
            return 0;
        }

        private static int ValidateIsoDate(string input)
        {
            int currentYear = DateTime.Now.Year;

            string numReg = "^[0-9]+$";
            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if ((input.Length) % 2 > 0 || input.Length > 12)
            {
                return 2;
            }
            Match match = Regex.Match(input, numReg, RegexOptions.IgnoreCase);
            if (!match.Success) return 2;
            else
            {
                string subYear = input.Substring(0, 4);
                string subMonth = "";
                string subDay = "";
                string subHour = "";
                string subMins = "";
                ////////////////
                int iYear = 0;
                iYear = int.Parse(subYear);

                var iMonth = 0;
                var iDay = 0;
                var iHour = 0;
                var iMins = 0;
                if (iYear < 1800 || iYear > (currentYear + 2))
                {
                    return 3;
                }
                if (input.Length > 4)
                {
                    subMonth = input.Substring(4, 2);
                    iMonth = int.Parse(subMonth);
                    if (iMonth < 1 || iMonth > 12)
                    {
                        return 2;
                    }
                }
                if (input.Length > 6)
                {
                    subDay = input.Substring(6, 2);
                    iDay = int.Parse(subDay);
                    if (iMonth != 2)
                    {

                        if (iDay > daysInMonth[iMonth - 1])
                        {
                            return 2;
                        }

                    }
                    else
                    {
                        if (isLeap(iYear))
                        {
                            if (iDay > 29)
                            {
                                return 2;
                            }
                        }
                        else
                        {
                            if (iDay > 28)
                            {
                                return 2;
                            }
                        }
                    }
                }
                if (input.Length > 8)
                {
                    subHour = input.Substring(8, 2);
                    iHour = int.Parse(subHour);
                    if (iHour >= 24)
                    {
                        return 2;
                    }
                }
                if (input.Length > 10)
                {
                    subMins = input.Substring(10, 2);
                    iMins = int.Parse(subMins);
                    if (iMins > 59)
                    {
                        return 2;
                    }
                }

            }
            return 0;
        }

        private static bool isLeap(int yy)
        {
            if ((yy % 400 == 0 || yy % 100 != 0) && (yy % 4 == 0))
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private static bool IsAnyPropNotNull<T>(T model)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());

            var subList = props.Where(item => !item.Name.Contains("_DATE") &&
                                               !item.Name.Contains("COMMENT") &&
                                               item.Name != "Id" &&
                                               item.Name != "DataStatus" &&
                                               !item.Name.Contains("UserId") &&
                                               !item.Name.Contains("Date") &&
                                               !item.Name.Contains("SiteId") &&
                                               !item.Name.Contains("GroupId") &&
                                               !item.Name.Contains("DataOrigin")).ToList();

            var isAnyVAlue = subList.Any(item => item.GetValue(model, null) != null);

            return isAnyVAlue;
        }

        private static bool XORNull<T>(string obja, string objb) where T : IComparable<T>
        {
            if (String.IsNullOrEmpty(obja) && !String.IsNullOrEmpty(objb))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(obja) && String.IsNullOrEmpty(objb))
            {
                return true;
            }
            return false;
        }

        private static bool IsValidPattern(string pattern, string regex)
        {
            Match match = Regex.Match(pattern, regex);
            return match.Success;
        }

        private static bool IsValidPlotString(string plot, int group)
        {
            if (String.Compare(plot, "outside_cp", true) == 0)
            {
                return true;
            }

            int[] allowedOutside = { 10, 17, 19, 21, 22 };
            List<int> notSP_II_Valid = new List<int>() { 21, 22 };
            bool isMatch = true;
            try
            {
                Match match;

                if (plot.ToLower() == "cp")
                {
                    match = Regex.Match(plot, Globals.cpReg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plot.ToLower() == "sp-i")
                {
                    match = Regex.Match(plot, Globals.sp1Reg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plot.ToLower() == "sp-ii")
                {
                    if (notSP_II_Valid.Any(id => id == group))
                    {
                        isMatch = false;
                    }
                    else
                    {
                        match = Regex.Match(plot, Globals.sp2Reg, RegexOptions.IgnoreCase);
                        isMatch = match.Success;
                    }
                }
                else
                {
                    if (String.Compare(plot, "outside_cp", true) == 0)
                    {
                        //check for which groups 'outside_cp' is allowed
                        if (allowedOutside.Contains(group))
                        {
                            isMatch = true;
                        }
                    }
                    else
                    {
                        isMatch = false;
                    }
                }
            }
            catch (Exception dd)
            {
                isMatch = false;
            }

            return isMatch;
        }

        private static bool CountBoundedProps<T>(T model, int bound, params string[] vars)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());

            var subList = props.Where(item => vars.Contains(item.Name)).ToList();

            var countValue = subList.Count(item => item.GetValue(model, null) != null);

            return (countValue == 0) || (countValue == bound);
        }

        /* public async Task<int> FlsmPlotIdInSamplingPointGroupAsync(GRP_FLSM model, int siteId, IcosDbContext _context)
         {
             var item = await _context.GRP_PLOT.Where(plot => plot.SiteId == siteId && plot.DataStatus == 0 &&
                                                 String.Compare(plot.PLOT_ID, model.FLSM_PLOT_ID) == 0 &&
                                                 String.Compare(plot.PLOT_DATE, model.FLSM_DATE) <= 0).FirstOrDefaultAsync();
             if (item == null)
             {
                 return (int)Globals.ErrorValidationCodes.FLSM_PLOT_ID_NOT_FOUND;
             }
             return 0;
         }

          public async Task<int> SosmPlotIdInSamplingPointGroupAsync(GRP_SOSM model, int siteId)
         {
             var item = await _context.GRP_PLOT.Where(plot => plot.SiteId == siteId && plot.DataStatus == 0 &&
                                                 String.Compare(plot.PLOT_ID, model.SOSM_PLOT_ID) == 0 &&
                                                 String.Compare(plot.PLOT_DATE, model.SOSM_DATE) <= 0).FirstOrDefaultAsync();
             if (item == null)
             {
                 return (int)Globals.ErrorValidationCodes.SOSM_PLOT_ID_NOT_FOUND;
             }
             return 0;
         }
        */

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
    }
}
