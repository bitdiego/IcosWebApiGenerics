using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.SamplingValidation
{
    public class SamplingValidation
    {
//        private static Response response = null;
  //      private static string Err = "";
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }


        public static async Task ValidateSamplingSchemeResponseAsync(GRP_PLOT samplingScheme, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(samplingScheme.PLOT_DATE, "PLOT_DATE", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_DATE", "$V0$", "PLOT_DATE", "$GRP$", "GRP_PLOT");
            }
            errorCode = GeneralValidation.MissingMandatoryData<string>(samplingScheme.PLOT_ID, "PLOT_ID", "GRP_PLOT");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_ID", "$V0$", "PLOT_ID", "$GRP$", "GRP_PLOT");
            }
            else
            {
                if (!GeneralValidation.IsValidPlotString(samplingScheme.PLOT_ID, samplingScheme.GroupId))
                {
                    errorCode = 10;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "PLOT_ID", "$V0$", "PLOT_ID", "$V1$", samplingScheme.PLOT_ID);
                }
                else
                {
                    /*if (!String.IsNullOrEmpty(samplingScheme.PLOT_REFERENCE_POINT))
                    {
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
                    }*/
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
                string _subPlot = samplingScheme.PLOT_ID.Substring(0, samplingScheme.PLOT_ID.IndexOf('_'));
                
                if (String.Compare(_subPlot, samplingScheme.PLOT_TYPE, true) != 0)
                {
                    errorCode = 1;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_TYPE", "$V0$", _subPlot, "$V1$", samplingScheme.PLOT_TYPE);
                }
            }

            //validate coordinate system:  PLOT_EASTWARD_DIST/PLOT_NORTHWARD_DIST or PLOT_ANGLE_POLAR/PLOT_DISTANCE_POLAR or PLOT_LOCATION_LAT/PLOT_LOCATION_LONG
            //must be mutually exclusive
            int xc = Globals.IsValidCoordinateSystem<decimal?>(samplingScheme.PLOT_EASTWARD_DIST, samplingScheme.PLOT_NORTHWARD_DIST,
                                                               samplingScheme.PLOT_DISTANCE_POLAR, samplingScheme.PLOT_ANGLE_POLAR,
                                                               samplingScheme.PLOT_LOCATION_LAT, samplingScheme.PLOT_LOCATION_LONG);
            if (xc < 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_EASTWARD_DIST");
            }
            else
            {
                if (xc < 2)
                {
                    //PLOT_EASTWARD_DIST/PLOT_NORTHWARD_DIST or PLOT_ANGLE_POLAR/PLOT_DISTANCE_POLAR have been submitted:
                    //must have also PLOT_REFERENCE_POINT and PLOT_NORTHREF
                    if(samplingScheme.PLOT_NORTHREF == null)
                    {
                        errorCode = 7;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_NORTHREF");
                    }
                    if (samplingScheme.PLOT_REFERENCE_POINT != null)
                    {
                        if (samplingScheme.PLOT_TYPE.ToLower() != "sp-ii" || !samplingScheme.PLOT_ID.ToLower().StartsWith("sp-ii"))
                        {
                            errorCode = 5;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                        }
                        
                    }
                    else
                    {
                        if (samplingScheme.PLOT_TYPE.ToLower() == "sp-ii" && samplingScheme.PLOT_ID.ToLower().StartsWith("sp-ii"))
                        {
                            errorCode = 5;
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                        }
                    }
                }
                else
                {
                    //PLOT_LOCATION_LAT/PLOT_LOCATION_LONG have been submitted:
                    //no need of PLOT_REFERENCE_POINT and PLOT_NORTHREF
                    if (samplingScheme.PLOT_NORTHREF != null || samplingScheme.PLOT_REFERENCE_POINT != null)
                    {
                        errorCode = 8;
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpSamplingSchemeErrors[errorCode], "PLOT_REFERENCE_POINT");
                    }
                }
            }
            

            //return response;
        }

        //validate also numeric values...
        public static async Task ValidateFlsmResponseAsync(GRP_FLSM flsm, IcosDbContext db, Response response)
        {
            //to do::: FLSM_PLOT_ID present in GRP_PLOT
            errorCode = await GeneralValidation .ItemInSamplingPointGroupAsync(flsm.FLSM_PLOT_ID, flsm.FLSM_DATE, flsm.SiteId, db);
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
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(flsm.FLSM_SAMPLE_TYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_SAMPLE_TYPE", "$V0$", flsm.FLSM_SAMPLE_TYPE, "$V1$", "FLSM_STYPE", "$GRP$", "GRP_FLSM");
                }*/
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
                /*errorCode = await GeneralValidation.ItemInBadmListAsync(flsm.FLSM_PTYPE, (int)Globals.CvIndexes.FLSM_STYPE, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FLSM_PTYPE", "$V0$", flsm.FLSM_PTYPE, "$V1$", "FLSM_PTYPE", "$GRP$", "GRP_FLSM");
                }*/
            }

            //return response;
        }

        public static async Task ValidateSosmResponse(GRP_SOSM sosm, IcosDbContext db, Response response)
        {
            //to do::: SOSM_PLOT_ID present in GRP_PLOT
            errorCode = await GeneralValidation.ItemInSamplingPointGroupAsync(sosm.SOSM_PLOT_ID, sosm.SOSM_DATE, sosm.SiteId, db);
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
           // return response;
        }
    }
}
