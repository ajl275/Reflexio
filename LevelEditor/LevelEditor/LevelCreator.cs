using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;


namespace LevelEditor
{
    /// <summary>
    /// This class will read in a xml file and create a level on the basis of that. 
    /// </summary>
    public static class LevelCreator
    {
        /*
         * The xml file format is as below:
         * <?xml version="1.0"?>
         * main tag: <Level> </Level>
         * Inside the level tag we have one tag for each object in the game. For example to
         * create a Player object, we shall do <Player></Player>. This shall automatically
         * create a new instance of the Player class.
         * Now suppose we want to set a variable called position in the object then
         * we would do
         * <Player>
         *  <Position>arguments</Position>
         * </Player>
         * 
         * This will basically create a new Player object (call it p) and
         * then will call p.SetPosition(arguments) OR p.AddPosition(arguments) or p.Position(arguments) whichever exists.
         * Arguments is a comma delimited list of values. The types of arguments parsed in order are:
         * 1) Hashed object - more down
         * 2) Booleans - 'true' or 'false'
         * 3) Ints
         * 4) Floats
         * 
         * You can go recursive for example
         * <Level>
         *  <Player>
         *      <Position>
         *          <X>0</X>
         *          <Y>0</Y>
         *      </Position>
         *  </Player>
         * </Level>
         * 
         * Will create a new Level object (lvl), then will create a new player object (ply),
         * then will create a new Position object (pos) and then will call the following methods:
         * pos.AddX("0") or pos.SetX("0")
         * pos.AddY("0") or pos.SetY("0")
         * ply.AddPosition(pos) or ply.SetPosition(pos)
         * lvl.AddPlayer(ply) or lvl.SetPlayer(ply).
         * 
         * 
         * TYPE Attribute:
         * Suppose we have a tag <PhysicsObject type = "Wall"> then this will create a new Wall
         * object and call level.AddPhysicsObject(wall). Here wall is a child of PhysicObject
         * 
         * HASH Attribute:
         * Suppose we have a tag <Level hash = "level1"> then the object created by calling new Level()
         * shall be stored in a hashtable and can be called later by specifying then name, i.e. 'level1'.
         * 
         * PARAMS Attribute:
         * Suppose we have an object that needs to be initialized with a constructor that takes an argument
         * of an object already created (e.g. Wall needs the level object) we can say:
         * <Wall params = "level1">
         * Where level1 is the hashname of the level object created before.
         * 
         * Textures:
         * To set a texture add tag <Texture>texture_name</Texture> where texture_name is stuff like
         * groundTexture, playerTexture, lineTexture, etc. See GameEngine for a complete list of names
         * and their corresponding files.
         * 
         * Initialize()
         * Always call Initialize() (<Initialize />) after setting all the parameters before adding
         * any objects like walls or players to the level.
         */

        /*
         * Reflection:
         * object obj = Activator.CreateInstance(Type.GetType("Reflexio." + class_name), args);
         * MethodInfo info = obj.GetType().GetMethod(method_name);
         * info.Invoke(obj, new object[] {arguments});
         * 
         * Similarly use FieldInfo, PropertyInfo
         */

        private const char SEPARATOR = ',';
        private static Hashtable object_names = new Hashtable();
        public static PhysicsObject CreateObjectFromName(string class_name, object[] args)
        {
            if (class_name.Equals("Level"))
            {
                return null;
            }

            PhysicsObject temp = new PhysicsObject(class_name,"","openDoorTexture","closeDoorTexture",0,0,1f,1f,.5f,true,"U");
            return temp;
        }

        public static void CallMethodFromName(object obj, string method_name, object[] arguments)
        {
            MethodInfo info = obj.GetType().GetMethod("Set" + method_name);
            if(info == null)
                info = obj.GetType().GetMethod("Add" + method_name);
            if(info == null)
                info = obj.GetType().GetMethod(method_name);
            if (info == null)
                return; //throw new System.Exception("The tag name " + method_name + " has no corresponding method. Please check level file.");
            info.Invoke(obj, arguments);
        }

        public static object[] ParseArguments(string args)
        {
            if (args.Length == 0)
                return null;
            string[] string_args = args.Split(new char[] { SEPARATOR });
            object[] arguments = new object[string_args.Length];
            for (int i = 0; i < string_args.Length; i++)
            {
                string_args[i] = string_args[i].Trim();
                int inum;
                float fnum;
                if (object_names.Contains(string_args[i]))
                    arguments[i] = object_names[string_args[i]]; // Has this been hashed?
                else if (string_args[i].Equals("true", StringComparison.OrdinalIgnoreCase))
                    arguments[i] = true; // Try bool
                else if (string_args[i].Equals("false", StringComparison.OrdinalIgnoreCase))
                    arguments[i] = false; // Try bool
                else if(Int32.TryParse(string_args[i], out inum))
                    arguments[i] = inum; // Try int
                else if(float.TryParse(string_args[i], out fnum))
                            arguments[i] = fnum; // Try float
                else
                    arguments[i] = string_args[i]; // Just keep it as a string
            }
            return arguments;
        }

        public static object ParseNodes(XmlElement element, Editor e)
        {
            object obj;
            object[] args = ParseArguments(element.GetAttribute("params"));
            obj = CreateObjectFromName(element.Name, args);

            if (obj == null)
            {
                obj = e;
            }
            else
            {
                if (((PhysicsObject)obj).type.Equals("DialogDisplay"))
                {
                    obj = Editor.dialog_displayer;
                }
                else
                {
                    e.objects.Add((PhysicsObject)obj);
                    if (((PhysicsObject)obj).type.Equals("Door"))
                    {
                        e.door = (PhysicsObject)obj;
                    }
                    if (((PhysicsObject)obj).type.Equals("Player"))
                    {
                        e.player = (PhysicsObject)obj;
                    }
                }                    
            }

            //if (element.HasAttribute("hash"))
            //    object_names.Add(element.GetAttribute("hash"), obj);

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        if (node.InnerXml.Contains("<"))
                        {
                            object arg = ParseNodes((XmlElement)node, e);
                            CallMethodFromName(obj, node.Name, new object[] { arg });
                        }
                        else
                        {
                            CallMethodFromName(obj, node.Name, ParseArguments(node.InnerText));
                        }
                        break;
                }
            }
            return obj;
        }

        public static void ParseLevelFromFile(String xmlfilename, Editor e)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlReader reader;
            try
            {
                reader = XmlReader.Create(xmlfilename);
            }
            catch
            {
                throw new System.Exception("There was an error. Please check if file exists.");
            }
            try
            {
                xmldoc.Load(reader);
            }
            catch
            {
                throw new System.Exception("The string is malformatted.");
            }
            XmlElement root = xmldoc.DocumentElement;

            ParseNodes(root, e);
        }
    }
}
