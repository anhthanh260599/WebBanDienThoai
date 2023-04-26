using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanDienThoai.Models
{
    [MetadataType(typeof(UserMasterData))]
    public partial class UserMaster
    {

    }

    [MetadataType(typeof(ProductMasterData))]
    public partial class ProductMaster
    {
        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }
    }


}