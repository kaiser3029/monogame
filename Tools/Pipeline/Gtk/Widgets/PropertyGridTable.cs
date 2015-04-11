﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace MonoGame.Tools.Pipeline
{
    public class FalseWidget
    {
        public object newvalue;

        public FalseWidget(object newvalue)
        {
            this.newvalue = newvalue;
        }
    }

    public class EventItem
    {
        public int id;
        public EventHandler eventHandler;

        public EventItem(int id, EventHandler eventHandler)
        {
            this.id = id;
            this.eventHandler = eventHandler;
        }
    }

    public class TreeItem
    {
        public string group;

        public string label;
        public object value;
        public PropertyGridTable.EntryType type;
        public EventHandler eventHandler;
        public Dictionary<string, object> comboItems;

        public TreeItem(string label, object value, PropertyGridTable.EntryType type, EventHandler eventHandler = null, Dictionary<string, object> comboItems = null)
        {
            group = "";

            this.label = label;
            this.value = value;
            this.type = type;
            this.eventHandler = eventHandler;
            this.comboItems = comboItems;
        }
    }

    [System.ComponentModel.ToolboxItem (true)]
    public partial class PropertyGridTable : VBox
    {
        Window window;

        TreeIter nulliter;
        TreeStore listStore;

        List<TreeItem> items;
        List<TreeItem> pitems;
        List<EventItem> eitems;

        public bool sortgroup;

        public void Initalize(Window window)
        {
            this.window = window;
        }

        public PropertyGridTable ()
        {
            Build();

            items = new List<TreeItem> ();
            pitems = new List<TreeItem> ();
            eitems = new List<EventItem> ();
            nulliter = new TreeIter ();
            sortgroup = true;

            var column1 = new TreeViewColumn ();
            column1.Sizing = TreeViewColumnSizing.Fixed;
            column1.FixedWidth = 100;
            column1.Resizable = true;
            column1.Title = "Name";

            var textCell1 = new CellRendererText ();
            textCell1.Underline = Pango.Underline.Single;
            column1.PackStart (textCell1, false);

            var textCell2 = new CellRendererText ();
            column1.PackStart (textCell2, false);

            var idCell = new CellRendererText ();
            idCell.Visible = false;
            column1.PackStart (idCell, false);

            treeview1.AppendColumn (column1);

            var column2 = new TreeViewColumn ();
            column2.Sizing = TreeViewColumnSizing.Fixed;
            column2.Resizable = true;
            column2.Title = "Value";

            var editTextCell = new CellRendererText ();

            editTextCell.Edited += delegate(object o, EditedArgs args) {
                #if GTK2
                TreeModel model;
                #elif GTK3
                ITreeModel model;
                #endif
                TreeIter iter;

                if (treeview1.Selection.GetSelected (out model, out iter)) {

                    int id = Convert.ToInt32(model.GetValue(iter, 11));

                    for(int i = 0;i < eitems.Count;i++)
                    {
                        if(eitems[i].id == id)
                        {
                            if(eitems[i].eventHandler != null)
                            {
                                var fwidget = new FalseWidget(args.NewText);
                                eitems[i].eventHandler(fwidget, EventArgs.Empty);
                                model.SetValue(iter, 4, args.NewText);
                                break;
                            }
                        }
                    }
                }
            };

            column2.PackStart (editTextCell, false);

            var editTextCell2 = new CellRendererText ();
            editTextCell2.Editable = true;
            editTextCell2.EditingStarted += delegate {
                #if GTK2
                TreeModel model;
                #elif GTK3
                ITreeModel model;
                #endif
                TreeIter iter;

                if (treeview1.Selection.GetSelected (out model, out iter)) {

                    var dialog = new CollectionEditorDialog(model.GetValue(iter, 14).ToString(), window);
                    dialog.TransientFor = window;
                    if(dialog.Run() == (int)ResponseType.Ok)
                    {
                        int id = Convert.ToInt32(model.GetValue(iter, 11));

                        for(int i = 0;i < eitems.Count;i++)
                        {
                            if(eitems[i].id == id)
                            {
                                if(eitems[i].eventHandler != null)
                                {
                                    var fwidget = new FalseWidget(dialog.text);
                                    eitems[i].eventHandler(fwidget, EventArgs.Empty);
                                    model.SetValue(iter, 14, dialog.text);
                                    break;
                                }
                            }
                        }
                    }
                }
            };
            column2.PackStart (editTextCell2, false);

            var editTextCell4 = new CellRendererText ();
            editTextCell4.Editable = true;
            editTextCell4.EditingStarted += delegate {
                #if GTK2
                TreeModel model;
                #elif GTK3
                ITreeModel model;
                #endif
                TreeIter iter;

                if (treeview1.Selection.GetSelected (out model, out iter)) {

                    var dialog = new ColorPickerDialog(model.GetValue(iter, 15).ToString());
                    dialog.TransientFor = window;
                    if(dialog.Run() == (int)ResponseType.Ok)
                    {
                        int id = Convert.ToInt32(model.GetValue(iter, 11));

                        for(int i = 0;i < eitems.Count;i++)
                        {
                            if(eitems[i].id == id)
                            {
                                if(eitems[i].eventHandler != null)
                                {
                                    var fwidget = new FalseWidget(dialog.data);
                                    eitems[i].eventHandler(fwidget, EventArgs.Empty);
                                    model.SetValue(iter, 15, dialog.data);
                                    break;
                                }
                            }
                        }
                    }
                }
            };
            column2.PackStart (editTextCell4, false);

            var editTextCell3 = new CellRendererText ();
            editTextCell3.Visible = false;
            column2.PackStart (editTextCell3, false);

            var comboCell = new CellRendererCombo ();
            comboCell.TextColumn = 0;
            comboCell.IsExpanded = true;
            comboCell.Editable = true;
            comboCell.HasEntry = false;
            comboCell.Edited += delegate(object o, EditedArgs args) {
                #if GTK2
                TreeModel model;
                #elif GTK3
                ITreeModel model;
                #endif
                TreeIter iter;

                if (treeview1.Selection.GetSelected (out model, out iter)) {

                    int id = Convert.ToInt32(model.GetValue(iter, 11));

                    for(int i = 0;i < eitems.Count;i++)
                    {
                        if(eitems[i].id == id)
                        {
                            if(eitems[i].eventHandler != null && args.NewText != null && args.NewText != "")
                            {
                                var fwidget = new FalseWidget(args.NewText);
                                eitems[i].eventHandler(fwidget, EventArgs.Empty);
                                model.SetValue(iter, 8, args.NewText);
                                break;
                            }
                        }
                    }
                }
            };

            column2.PackStart (comboCell , false);

            treeview1.AppendColumn (column2);

            column1.AddAttribute (textCell1, "text", 0);
            column1.AddAttribute (textCell1, "visible", 1);
            column1.AddAttribute (textCell2, "text", 2);
            column1.AddAttribute (textCell2, "visible", 3);
            column2.AddAttribute (editTextCell, "text", 4);
            column2.AddAttribute (editTextCell, "visible", 5);
            column2.AddAttribute (editTextCell, "editable", 6);
            column2.AddAttribute (comboCell, "model", 7);
            column2.AddAttribute (comboCell, "text", 8);
            column2.AddAttribute (comboCell, "editable", 9);
            column2.AddAttribute (comboCell, "visible", 10);
            column1.AddAttribute (idCell, "text", 11);
            column2.AddAttribute (editTextCell2, "text", 12);
            column2.AddAttribute (editTextCell2, "visible", 13);
            column2.AddAttribute (editTextCell3, "text", 14);
            column2.AddAttribute (editTextCell4, "text", 15);
            column2.AddAttribute (editTextCell4, "visible", 16);

            listStore = new TreeStore (typeof (string), typeof(bool), typeof (string), typeof(bool), typeof (string), typeof(bool), typeof(bool), typeof(TreeStore), typeof(string), typeof(bool), typeof(bool), typeof(string), typeof(string), typeof(bool), typeof(string), typeof(string), typeof(bool));
            treeview1.Model = listStore;
        }

        TreeIter AddGroup(string name)
        {
            return listStore.AppendValues (name, true, "", false, "", false, false, null, "", false, false, "", "", false, "", "", false);
        }

        TreeIter AddPropertyTextBox(TreeIter iter, string id, string name, string text, bool editable)
        {
            return !iter.Equals(nulliter) ? 
                listStore.AppendValues(iter, "", false, name, true, text, true, editable, null, "", false, false, id, "", false, "", "", false) : 
                listStore.AppendValues("", false, name, true, text, true, editable, null, "", false, false, id, "", false, "", "", false);
        }

        TreeIter AddPropertyCollectionBox(TreeIter iter, string id, string name, string data)
        {
            return !iter.Equals(nulliter) ? 
                listStore.AppendValues(iter, "", false, name, true, "", false, false, null, "", false, false, id, "Collection", true, data, "", false) : 
                listStore.AppendValues("", false, name, true, "", false, false, null, "", false, false, id, "Collection", true, data, "", false);
        }

        TreeIter AddPropertyColorBox(TreeIter iter, string id, string name, string color)
        {
            return !iter.Equals(nulliter) ? 
                listStore.AppendValues(iter, "", false, name, true, "", false, false, null, "", false, false, id, "", false, "", color, true) : 
                listStore.AppendValues("", false, name, true, "", false, false, null, "", false, false, id, "", false, "", color, true);
        }

        TreeIter AddPropertyComboBox(TreeIter iter, string id, string name, string[] values, string text)
        {
            var store = new TreeStore (typeof (string));

            foreach (string value in values)
                store.AppendValues(value);

            return !iter.Equals(nulliter) ? 
                listStore.AppendValues(iter, "", false, name, true, "", false, false, store, text, true, true, id, "", false, "", "", false) : 
                listStore.AppendValues("", false, name, true, "", false, false, store, text, true, true, id, "", false, "", "", false);
        }

        TreeIter AddPropertyComboBox(TreeIter iter, string id, string name, object store, string text)
        {
            return !iter.Equals(nulliter) ? 
                listStore.AppendValues(iter, "", false, name, true, "", false, false, store, text, true, true, id, "", false, "", "", false) : 
                listStore.AppendValues("", false, name, true, "", false, false, store, text, true, true, id, "", false, "", "", false);
        }

        public enum EntryType {
            Text,
            Readonly,
            LongText,
            Color,
            Combo,
            Check,
            Integer,
            List,
            Unkown
        }

        void SortABC()
        {
            items = items.OrderBy (o => o.label).ToList ();
            pitems = pitems.OrderBy (o => o.label).ToList ();
        }

        void SortGroup()
        {
            var ilist = new List<List<TreeItem>> ();

            foreach (TreeItem item in items) {
                bool added = false;

                foreach (List<TreeItem> silist in ilist) {
                    if (silist [0].group == item.group) {
                        silist.Add (item);
                        added = true;
                    }
                }

                if (!added) {
                    var newlist = new List<TreeItem> ();
                    newlist.Add (item);
                    ilist.Add (newlist);
                }
            }

            ilist = ilist.OrderBy (o => o[0].group).ToList ();

            items.Clear ();
            foreach (List<TreeItem> silist in ilist) 
                foreach (TreeItem item in silist) 
                    items.Add (item);
        }

        public void Refresh()
        {
            SortABC ();

            if (sortgroup)
                SortGroup ();

            listStore.Clear ();
            eitems.Clear ();

            string group = "";
            var groupiter = new TreeIter();

            foreach (TreeItem item in items) {
                TreeIter iter;

                if (sortgroup) {
                    if (item.group != group) {
                        group = item.group;
                        groupiter = AddGroup (item.group);
                    }
                }

                iter = !sortgroup ? AddTreeItem(item, nulliter) : AddTreeItem(item, groupiter);

                if (item.label == "Processor") 
                    foreach (TreeItem pitem in pitems) 
                        AddTreeItem (pitem, iter);
            }


            treeview1.ExpandAll();
            treeview1.Columns[0].FixedWidth = Allocation.Width / 2;
            treeview1.Columns[1].FixedWidth = Allocation.Width / 2;
        }

        public TreeIter AddTreeItem(TreeItem item, TreeIter iter)
        {
            var eitem = new EventItem (eitems.Count, item.eventHandler);
            eitems.Add (eitem);

            if (item.type == EntryType.Readonly || item.type == EntryType.Text || item.type == EntryType.LongText || item.type == EntryType.Integer) {
                string text;
                bool editable = true;

                text = item.value as string ?? "";
                if (item.value is char)
                    text = item.value.ToString () ?? "";

                editable &= item.type != EntryType.Readonly;

                return AddPropertyTextBox (iter, eitem.id.ToString (), item.label, text, editable);
            } else if (item.type == EntryType.Color) {
                var c = (Microsoft.Xna.Framework.Color)item.value;

                return AddPropertyColorBox (iter, eitem.id.ToString (), item.label, c.ToString ());
            } else if (item.type == EntryType.Combo) {
                TreeStore model = null;

                foreach (var v in item.comboItems) {
                    if (model == null)
                        model = new TreeStore (v.Key.GetType (), v.Value.GetType ());
                    model.AppendValues (v.Key, v.Value);
                }

                return AddPropertyComboBox (iter, eitem.id.ToString (), item.label, model, item.value.ToString ());
            } else if (item.type == EntryType.Check) {
                return AddPropertyComboBox (iter, eitem.id.ToString (), item.label, new [] { "True", "False" }, item.value.ToString ());
            } else if (item.type == EntryType.List) {
                var values = (List<string>)item.value;
                string text = "";

                if (values.Count > 0) {
                    text = values [0];
                    for (int i = 1; i < values.Count; i++) 
                        text += "\r\n" + values [i];
                }

                return AddPropertyCollectionBox (iter, eitem.id.ToString (), item.label, text);
            }
            else 
                return AddPropertyTextBox (iter, eitem.id.ToString (), item.label, "", false);
        }

        public void Clear() {
            items.Clear ();
            pitems.Clear ();
            eitems.Clear ();
            listStore.Clear ();
        }

        public void AddEntry(string label, object value, EntryType type, EventHandler eventHandler = null, Dictionary<string, object> comboItems = null) {

            var item = new TreeItem (label, value, type, eventHandler, comboItems);

            if (item.label == "Name" || item.label == "Location")
                item.group = "Common";
            else
                item.group = "Settings";

            items.Add (item);
        }

        public void AddProcEntry(string label, object value, EntryType type, EventHandler eventHandler = null, Dictionary<string, object> comboItems = null) {

            pitems.Add (new TreeItem (label, value, type, eventHandler, comboItems));
        }
    }
}

