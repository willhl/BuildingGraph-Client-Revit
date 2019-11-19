using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Client.Model
{


    public class AbstractElement : Node
    {

    }

    public class ElementType : AbstractElement
    {
        public string Category { get; set; }



    }

    public class ModelElement : Node
    {
        public string Category { get; set; }

    }

    public class Space : AbstractElement
    {

    }

    public class Project : Node
    {
        public string number { get; set; }
    }

    public class RevitModel : Node
    {
        public string uri { get; set; }

        public override string Label => "Model";
    }

    public class ForgeModel : Node
    {
        public string uri { get; set; }
    }

    public  class Surface : AbstractElement
    {
        public double area { get; set; }
    }

    public class VoidVolume : Node
    {

    }

    public class Wall : AbstractElement
    {

    }

    public class Door : AbstractElement
    {

    }

    public class Furniture : AbstractElement
    {

    }


    public class Window : AbstractElement
    {

    }

    public class Ceiling : AbstractElement
    {

    }

    public class Column : AbstractElement
    {

    }


    public class Roof : AbstractElement
    {

    }   

    public class Floor : AbstractElement
    {

    }

    public class Section : AbstractElement
    {

    }

    public class Duct : Section
    {

    }

    public class Pipe : Section
    {

    }

    public class CableTray : Section
    {

    }

    public class Transition : AbstractElement
    {

    }

    public class DuctTransition : Transition
    {

    }
    public class PipeTransition : Transition
    {

    }

    public class CableTrayTransition : Transition
    {

    }

    public class Terminal : AbstractElement
    {

    }

    public class Accessory : AbstractElement
    {

    }

    public class DuctAccessory : Accessory
    {

    }
    public class PipeAccessory : Accessory
    {

    }

    public class Fixture : Accessory
    {

    }

    public class Equipment : AbstractElement
    {

    }

    public class Level : AbstractElement
    {

    }


    public class System : AbstractElement
    {

    }

    public class Circuit : AbstractElement
    {

    }


    public class DBPanel : AbstractElement
    {

    }

    public class ElectricalLoad : AbstractElement
    {

    }

    public class ElectricalSource : AbstractElement
    {

    }

    public class ElectricalDevice : AbstractElement
    {

    }

    public class Data : AbstractElement
    {

    }

    public class Sensor : AbstractElement
    {

    }

    public class Control : AbstractElement
    {

    }


    public class Security : AbstractElement
    {

    }

    public class Safety : AbstractElement
    {

    }

    public class Communications : AbstractElement
    {

    }

    public class Sprinkler : Safety
    {

    }

    public class FireAlarm : Safety
    {

    }


    public class Lighting : AbstractElement
    {

    }
    public class Environment : AbstractElement
    {

    }


    public class Realisable : Node
    {

    }


    public class RealisableType : Node
    {

    }

}
