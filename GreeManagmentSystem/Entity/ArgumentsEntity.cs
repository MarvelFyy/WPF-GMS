using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeManagmentSystem.Entity
{
    class ArgumentsEntity
    {
        //-------名称---------   

        //类别
        public string CategoryVal { get; set; }

        //系列
        public string SeriesVal { get; set; }

        //工作方式
        public string WorkVal { get; set; }

        //能效等级
        public string LevelVal { get; set; }

        //能暖方式
        public string ColdVal { get; set; }

        //颜色/备注
        public string ColorVal { get; set; }

        //-------型号---------

        //产品代号
        public string ProCodeVal { get; set; }

        //结构形式
        public string StructVal { get; set; }

        //功能代号
        public string FunctionVal { get; set; }

        //规格代号
        public string SpecificationVal { get; set; }

        //类别代号
        public string CategoryCodeVal { get; set; }

        //外机结构
        public string ExternalVal { get; set; }

        //冷量 结构 序号
        public string MultipleVal { get; set; }

        //工作方式
        public string WorkCodeVal { get; set; }

        //冷暖方式
        public string ColdCodeVal { get; set; }

        //面板代码
        public string FaceCodeVal { get; set; }

        //能效等级
        public string LevelCodeVal { get; set; }

        //备注信息
        public string RemarkCodeVal { get; set; }


    }
}
