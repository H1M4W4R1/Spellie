using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.MovementTypes;
using Spellie.MovementTypes.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using UnityEngine;

namespace Spellie.Tools.Parsing
{
    public static class ISLParser
    {

        /// <summary>
        /// True if has been initialized
        /// </summary>
        private static bool _initialized;

        /// <summary>
        /// Switches logging of spell creation info
        /// </summary>
        public static bool logEnabled = true;
        
        /// <summary>
        /// Known spell elements
        /// </summary>
        public static Dictionary<string, Type> knownSpellElements = new Dictionary<string,Type>();
        
        /// <summary>
        /// Known Movement Types
        /// </summary>
        public static Dictionary<string,Type> knownMovements = new Dictionary<string,Type>();

        public static void Initialize()
        {
            
            if (!_initialized)
            {
                var assembly = Assembly.GetExecutingAssembly();
                foreach (var type in assembly.GetTypes())
                {
                    // Parse Spellie Elements
                    var attributes = type.GetCustomAttributes(typeof(SpellieElementAttribute), false);
                    if (attributes.Length > 0)
                    {
                        if (attributes[0] is SpellieElementAttribute attribute) knownSpellElements.Add(attribute.name, type);
                    }
                    
                    // Parse Spellie Movements
                    var attributes2 = type.GetCustomAttributes(typeof(SpellieMovementAttribute), false);
                    if (attributes2.Length > 0)
                    {
                        if (attributes2[0] is SpellieMovementAttribute attribute) knownMovements.Add(attribute.name, type);
                    }
                }

                if (logEnabled)
                {
                    Debug.Log($"Found: {knownSpellElements.Count} Spell Elements & {knownMovements.Count} Movement Types");
                    
                }
                
                _initialized = true;
            }
        }
        
        /// <summary>
        /// Parses spell from ISL code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Spell ParseSpell(string code)
        {
            var lineNo = 0;
            Initialize();
        
            // Result
            ISpellieElement parsedData = null;
            
            // Last parsed object, stores objects in memory
            var lastObject = new List<object>();
            var lastSpaces = -1;
            var wasOpening = false;
            
            // For all lines in code
            foreach (var v in code.Split('\n'))
            {
                lineNo++;
                var line = v;
                var amoSpaces = 0;
                while (line.StartsWith(" "))
                {
                    line = line.Remove(0, 1);
                    amoSpaces++;
                }
                
                if(logEnabled) Debug.Log("[SpellieParser] Parsing: " + line);

                // Remove objects that has been ended
                while (
                    (amoSpaces < lastSpaces && !wasOpening) ||
                    (amoSpaces <= lastSpaces && wasOpening)
                    )
                {
                    // Get last object
                    var l = lastObject[lastObject.Count - 1];
                    if (l is ISpellieElement)
                    {
                        // Invoke init method for ISpellieElement
                        l.GetType().GetMethod("Init")?.Invoke(l, null);
                    }
                    else if (l is IMovementType)
                    {
                        // Invoke build method for IMovementType
                        l.GetType().GetMethod("Build").Invoke(l, null);
                    }
                    else if (l is SpellieEvent)
                    {
                        l.GetType().GetMethod("Init").Invoke(l, null);
                    }
                    
                    // Remove last object
                    lastObject.RemoveAt(lastObject.Count - 1);
                    lastSpaces--;
                }

                // Ignore empty lines
                if (string.IsNullOrEmpty(line)) continue;
                
                // Split line into data and params
                var split = line.Split(':');
                
                // Get command
                var command = split[0];
                command = command.Trim(' ', '\r');
                var data = "";
                if (split.Length > 1)
                {
                    data = split[1];
                    data = data.Trim('\"', '\r');
                }

                if(logEnabled) Debug.Log("[SpellieParser] Command: \"" + command + "\" with data: \"" + data + "\"");
                
                // Get param (SSL is single-param based)
                var param = data.Replace(" ", "");

                // Parse Spell Elements
                try
                {
                    var spellieElement = knownSpellElements.FirstOrDefault(element => element.Key == command).Value;
                    if (spellieElement != null)
                    {
                        //var element = Activator.CreateInstance(spellieElement);
                        var builder = spellieElement.GetMethod("New")?.Invoke(null, new object[] { });

                        if (lastObject.Count > 0)
                        {
                            var lObj = lastObject[lastObject.Count - 1];

                            // Check if is SpellieElement
                            if (builder is ISpellieElement o)
                            {
                                // Check if assigning against event
                                if (lObj is SpellieEvent sEvent)
                                {
                                    sEvent.AddElement(o);
                                    if(logEnabled) Debug.Log("[SpellieParser] Adding element " + o.GetType() + " as result of event " +
                                              sEvent.name);
                                }
                                else
                                {
                                    throw new SyntaxErrorException(
                                        "[Spellie] That is not valid. You can assign Spellie Elements on events (only).");
                                }

                                // ISpellieElement
                                lastObject.Add(o);
                            }
                            else
                            {
                                throw new SyntaxErrorException("[Spellie] That is not a ISpellieElement. " +
                                                               "Something went really wrong. " +
                                                               "Have you assigned [SpellieElement] to non-element class? Line: " +
                                                               lineNo);
                            }
                        }
                        else
                        {
                            // Check if is SpellieElement
                            if (builder is ISpellieElement o)
                            {
                                parsedData = builder as ISpellieElement;
                                if(logEnabled) Debug.Log("[SpellieParser] Assigning element " + o.GetType() + " as spell base");

                            }
                            else
                            {
                                throw new SyntaxErrorException(
                                    "[Spellie] That is not valid. You need to begin spell using ISpellieElement like CastedElement.");
                            }

                            // ISpellieElement
                            lastObject.Add(o);
                            
                        }
                        wasOpening = true;
                        goto end;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    // Do nothing :)
                }

                // Parse spell movements
               
                if (command == "movement")
                {
                    try
                    {
                        var spellieMovement = knownMovements.FirstOrDefault(element => element.Key == param).Value;
                        if (spellieMovement != null)
                        {
                            //var element = Activator.CreateInstance(spellieMovement);
                            var builder = spellieMovement.GetMethod("New")?.Invoke(null, new object[] { });

                            var lObj = lastObject[lastObject.Count - 1];
                            if(logEnabled) Debug.Log("[SpellieParser] Assigning movement of type " + builder?.GetType() + " to " +
                                      lObj.GetType());

                            if (lObj is ISpellieElement)
                                (lObj as MetaObject)?.Set("movement", builder);
                            else
                                throw new SyntaxErrorException(
                                    "[Spellie] Movement types can be assigned only on ISpellieElement Meta-Objects. Line: " +
                                    lineNo);

                            // IMovementType
                            lastObject.Add(builder);
                            wasOpening = true;
                            goto end;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Nothing
                        Debug.LogError(ex);
                    }
                }

                // Parse attributes
                var currentObject = lastObject[lastObject.Count - 1];
                if (!command.StartsWith("on"))
                {
                    var fields = currentObject.GetType().GetFields()
                        .Where(f => f.GetCustomAttributes(typeof(SpelliePropertyAttribute), true).Length > 0);
                    var field = fields.FirstOrDefault(f =>
                        (f.GetCustomAttributes(typeof(SpelliePropertyAttribute), true)[0] as SpelliePropertyAttribute)
                        ?.Name == command);

                    if (field != null)
                    {
                        var type = field.FieldType;
                        
                        // Trim triangle from object definitions
                        if (command == "object")
                            param = param.Trim('<', '>');
                        
                        var value = Utility.FromString(param, type);
                        
                        
                            

                        (currentObject as MetaObject)?.Set(command, value);
                        
                        if(logEnabled) Debug.Log("[SpellieParser] Setting meta-object value of " + command + "(" + type + ") to " + value);

                        //field.SetValue(currentObject, );
                    }
                    else throw new SyntaxErrorException("[Spellie] Property is not valid [" + command +"] on [" + currentObject.GetType() + "]. Line: " + lineNo);

                    wasOpening = false;
                }
                else
                {
                    var e = new SpellieEvent();
                    e.name = command;
                    
                    // Add event to SpellieElement
                    if(lastObject.Count <= 0)
                        throw new SyntaxErrorException("[Spellie] Base command of spell should be ISpellieElement-related.");
                        
                    var lObj = lastObject[lastObject.Count - 1];
                    if (lObj is ISpellieElement)
                    {
                        // Add event to spellie element
                        (lObj as ISpellieElement)?.AddEventBase(e);
                    }
                    else
                    {
                        Debug.LogError("Errored on line: " + line);
                        throw new SyntaxErrorException("[Spellie] Events can be added only onto ISpellieElement");
                    }
                    
                    // Register event as last object
                    lastObject.Add(e);
                    wasOpening = true;
                }
  
                end:
                lastSpaces = amoSpaces;
            }

            // Initialize rest of objects :)
            while (lastObject.Count > 0)
            {
                // Get last object
                var l = lastObject[lastObject.Count - 1];
                if (l is ISpellieElement)
                {
                    // Invoke init method for ISpellieElement
                    l.GetType().GetMethod("Init")?.Invoke(l, null);
                    
                }
                else if (l is IMovementType)
                {
                    // Invoke build method for IMovementType
                    l.GetType().GetMethod("Build").Invoke(l, null);
                }
                else if (l is SpellieEvent)
                {
                    l.GetType().GetMethod("Init").Invoke(l, null);
                }
                    
                // Remove last object
                lastObject.RemoveAt(lastObject.Count - 1);
            }
            
            // Build parsed spell
            var spell = new Spell().WithName("Spellie::ParsedSpell")
                .WithCastElement(parsedData);

            return spell;
        }
        
    }
}