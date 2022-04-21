using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB
{
    /// <summary>
    /// 中标信息
    /// </summary>
    public class BidInfo
    {
        /// <summary>
        /// 中标代码
        /// </summary>
        public string BidCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 标段
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 公告时间
        /// </summary>
        public string PublicityTime { get; set; }

        /// <summary>
        /// 招标人
        /// </summary>
        public string Tenderee { get; set; }

        /// <summary>
        /// 招标代理机构
        /// </summary>
        public string Agency { get; set; }

        /// <summary>
        /// 招标方式
        /// </summary>
        public string BiddingMethod { get; set; }

        /// <summary>
        /// 中标人
        /// </summary>
        public string Bidder { get; set; }

        /// <summary>
        /// 中标价
        /// </summary>
        public string BidPrice { get; set; }

        /// <summary>
        /// 中标工期
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public string ProjectManager { get; set; }

        /// <summary>
        /// 资格等级
        /// </summary>
        public string QualificationLevel { get; set; }

        /// <summary>
        /// 资格证书编号
        /// </summary>
        public string QualificationCode { get; set; }

        /// <summary>
        /// 是否暂定金额
        /// </summary>
        public string PendingAmount { get; set; }

    }
}
