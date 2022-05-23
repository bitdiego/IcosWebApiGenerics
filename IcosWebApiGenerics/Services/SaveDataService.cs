using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services
{
    public class SaveDataService<T> : ISaveDataService<T> where T : BaseClass, new()
    {
        private readonly IcosDbContext _context;
        public SaveDataService(IcosDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ItemInDbAsync(T t)
        {
            bool res = false;
            bool isIdentical = false;
            switch (t.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION:
                    GRP_LOCATION loc = t as GRP_LOCATION;
                    var iteml = await _context.GRP_LOCATION.FirstOrDefaultAsync(l => l.SiteId == loc.SiteId && l.DataStatus == 0);

                    if (iteml != null)
                    {
                        //only one record for location group
                        res = isIdentical = iteml == loc;
                        if (!isIdentical)
                        {
                            //mark existing record as invalid
                            await SetItemInvalidAsync(t);
                        }
                    }
                    break;
                case (int)Globals.Groups.GRP_INST:
                    GRP_INST inst = t as GRP_INST;
                    //var _item = await _context.GRP_INST.Where(x => x.INST_MODEL == inst.INST_MODEL && x.INST_SN == x.INST_SN && x.DataStatus == 0 && x.SiteId == inst.SiteId).FirstOrDefaultAsync();
                    var _item = await _context.GRP_INST.Where(x => x.INST_MODEL == inst.INST_MODEL && x.INST_SN == x.INST_SN && x.INST_FACTORY == inst.INST_FACTORY 
                                                                && x.DataStatus == 0 && x.SiteId == inst.SiteId).FirstOrDefaultAsync();

                    if (_item != null)
                    {
                        res = isIdentical = _item == inst;
                        if (!isIdentical)
                        {
                            //what is changed??? comment? operation ? 
                            //for inst, there must be only a purchase item: so model, sn, date and factory operation are the same, 
                            //and if comment, calib function, firmware are changed -> update record
                        }


                    }
                    break;
            }
            return res;
        }

        public async Task<T> ItemInDbAsync(T t, int siteId)
        {
            
            int res = 0;
            dynamic xitem = null;
            Type xt=t.GetType();
            string xs = xt.ToString();
            bool isIdentical = false;

            switch (t.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION:
                    GRP_LOCATION loc = t as GRP_LOCATION;
                    var iteml = await _context.GRP_LOCATION.FirstOrDefaultAsync(l => l.SiteId == loc.SiteId && l.DataStatus == 0);

                    if (iteml != null)
                    {
                        isIdentical = iteml == loc;
                        if (!isIdentical)
                        {
                            //mark existing record as invalid
                            await SetItemInvalidAsync(t);
                        }
                        xitem = iteml;
                    }
                break;
                case (int)Globals.Groups.GRP_INST:
                    GRP_INST inst = t as GRP_INST;
                    //var _item = await _context.GRP_INST.Where(x => x.INST_MODEL == inst.INST_MODEL && x.INST_SN == x.INST_SN && x.DataStatus == 0 && x.SiteId == inst.SiteId).FirstOrDefaultAsync();
                    var _item = await _context.GRP_INST.Where(x => x.INST_MODEL == inst.INST_MODEL && x.INST_SN == x.INST_SN && x.DataStatus == 0 && x.SiteId == inst.SiteId).ToListAsync();

                    if (_item != null)
                    {
                        isIdentical = _item.Any(xinst => xinst == inst);
                        //isIdentical = _item == inst;
                        if (!isIdentical)
                        {
                            //what is changed??? comment? operation ? 
                        }
                        
                        xitem = _item;
                        res = 0;
                    }
                    break;
                /*
                case (int)Globals.Groups.GRP_DM:
                    GRP_DM dm = t as GRP_DM;
                    
                    break;
                case (int)Globals.Groups.GRP_PLOT:
                    GRP_PLOT plot = t as GRP_PLOT;
                    var _plot = await _context.GRP_PLOT.FirstOrDefaultAsync(plt => plt.PLOT_ID == plot.PLOT_ID && plt.SiteId == plot.SiteId && plt.DataStatus == 0);
                    if (_plot != null)
                    {
                        xitem = _plot;
                    }
                    break;
                case (int)Globals.Groups.GRP_INST:
                    GRP_INST inst = t as GRP_INST;
                    var item = await _context.GRP_INST.FirstOrDefaultAsync(md => md.INST_MODEL == inst.INST_MODEL && md.INST_SN == inst.INST_SN
                                                                && md.INST_FACTORY == inst.INST_FACTORY && md.SiteId == inst.SiteId && md.DataStatus == 0);
                    bool isEqual = item == inst;
                    if (isEqual)
                    {
                        res = item.Id;
                        xitem = item;
                    }
                    break;
                
                case (int)Globals.Groups.GRP_TOWER:
                    GRP_TOWER tower = t as GRP_TOWER;
                    var itemt = await _context.GRP_TOWER.FirstOrDefaultAsync(t => t.SiteId == tower.SiteId && t.DataStatus == 0);
                    if (itemt != null)
                        xitem = itemt;
                    break;
                case (int)Globals.Groups.GRP_CLIM_AVG:
                    GRP_CLIM_AVG climavg = t as GRP_CLIM_AVG;
                    var itemc = await _context.GRP_CLIM_AVG.FirstOrDefaultAsync(c => c.SiteId == climavg.SiteId && c.DataStatus == 0);
                    if (itemc != null)
                        xitem = itemc;
                    break;
                case (int)Globals.Groups.GRP_DHP:
                    GRP_DHP dhp = t as GRP_DHP;
                    var itemd = await _context.GRP_DHP.FirstOrDefaultAsync(d => d.SiteId == dhp.SiteId && d.DHP_ID == dhp.DHP_ID && d.DataStatus == 0);
                    if (itemd != null)
                        xitem = itemd;
                    break;
                case (int)Globals.Groups.GRP_BULKH:
                    GRP_BULKH bulkh = t as GRP_BULKH;
                    var itemb = await _context.GRP_BULKH.FirstOrDefaultAsync(b => b.SiteId == bulkh.SiteId && b.BULKH_PLOT == bulkh.BULKH_PLOT && bulkh.BULKH_DATE == b.BULKH_DATE && b.DataStatus == 0);
                    if (itemb != null)
                        xitem = itemb;
                    break;*/
            }
            return xitem;
        }

        public async Task<bool> UpdateItemAsync(int? id, int siteId, int userId, T t)
        {
            bool b = false;
            if (id == null || siteId < 1) return false;
            var item = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id && x.SiteId == siteId);
            if (item != null)
            {
                b = await SetItemInvalidAsync(siteId, userId, item);
            }
            if (b)
            {
                t.Id = 0;
                t.DataStatus = 0;
                t.InsertUserId = userId;
                t.SiteId = siteId;
                t.InsertDate = DateTime.Now;
                _context.Set<T>().Add(t);
                int res = await _context.SaveChangesAsync();
            }
            return b;
        }
        public async Task<bool> SetItemInvalidAsync(int siteId, int userId, T t)
        {
            int res = 0;
            t.DataStatus = 2;
            t.DeletedDate = DateTime.Now;
            t.DeleteUserId = userId;
            res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> SetItemInvalidAsync(T t)
        {
            int res = 0;
            t.DataStatus = 2;
            t.DeletedDate = DateTime.Now;
            t.DeleteUserId = t.InsertUserId;
            res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> SaveItemAsync(T t, int insertUserId, int siteId)
        {
            if (siteId < 1) return false;
            t.DataStatus = 0;
            t.InsertUserId = insertUserId;
            t.SiteId = siteId;
            t.InsertDate = DateTime.Now;
            t.DataOrigin = 1;
            // var item = await ItemInDbAsync(t, siteId);
            var isRecordPresent = await ItemInDbAsync(t);
            if (isRecordPresent) return true;
            /*if (item != null)
            {
                bool b = await SetItemInvalidAsync(siteId, insertUserId, item);
                if (!b) return false;
            }
            */
            _context.Set<T>().Add(t);
            int res = await _context.SaveChangesAsync();
           
            return res > 0;
        }
    }
}
