﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Configuration;
using BoardGameDesigner.Projects;
using BoardGameDesigner.Designs;
namespace BoardGameDesigner
{
    /// <summary>
    /// Interaction logic for ProjectManager.xaml
    /// </summary>
    public partial class ProjectManager : Window
    {
        #region Variables
        private UserControl _activeControl;
        private IProject CurrentProject;
        public DependencyProperty ProjectLoaded = DependencyProperty.Register("ProjectLoaded", typeof(bool), typeof(Window));
        
        #endregion

        #region Constructors
        public ProjectManager(IProject proj, string projFilePath)
        {
            CurrentProject = proj;
            proj.ProjectFilePath = projFilePath;
            InitializeComponent();
            if (proj != null)
                SetCurrentValue(ProjectLoaded, true);
        }
        public ProjectManager()
        {
            InitializeComponent();
            SetCurrentValue(ProjectLoaded, false);
        }
        #endregion

        #region Load Event
        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {
            //Populate tree view with all current elements
            RefreshTreeView();
        }
        #endregion

        #region Tree View Manipulation
        private bool LookupExpandedFlag(Dictionary<TreeViewItem, bool> dict, string value)
        {
            var applicableVals = dict.Keys.Where(item => (string)item.Header == value);
            if (applicableVals == null || applicableVals.Count() == 0)
                return false;
            return dict[dict.Keys.Where(item => (string)item.Header == value).First()];
        }
        private Dictionary<TreeViewItem, bool> PopulateDictionaryWithExpanded(ItemCollection collection)
        {
            var dict = new Dictionary<TreeViewItem, bool>();
            foreach(TreeViewItem item in collection)
            {
                if (item.Items.Count > 0)
                {
                    var subDict = PopulateDictionaryWithExpanded(item.Items);
                    foreach (KeyValuePair<TreeViewItem, bool> pair in subDict)
                    {
                        dict.Add(pair.Key, pair.Value);
                    }
                }
                dict.Add(item, item.IsExpanded);
            }

            return dict;
        }
        private TreeViewItem CreateTreeViewItem(string name, object dataContext, bool shouldLookupExpandedFlag, Dictionary<TreeViewItem, bool> dict = null)
        {
            var treeViewItem = new TreeViewItem() { Header = name, DataContext = dataContext };
            if (dataContext is IToggleable)
            {
                if ((dataContext as IToggleable).Enabled == false)
                {
                    treeViewItem.Header = name + " (Disabled)";
                }
            }
            if (shouldLookupExpandedFlag && dict != null)
            {
                treeViewItem.IsExpanded = LookupExpandedFlag(dict, name);
            }
            treeViewItem.MouseMove += TreeViewItem_MouseMove;
            treeViewItem.MouseDoubleClick += treeViewItem_MouseDoubleClick;
            return treeViewItem;
        }

        void treeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = tvElementView.SelectedItem;
            if (selectedItem != null)
            {
                var dataContext = (selectedItem as FrameworkElement).DataContext;
                var isDesignElement = dataContext is IDesignElement;
                var isDesign = dataContext is IDesign;
                var isDataset = dataContext is DataSet;
                var isDataTable = dataContext is DataTable;
                var isCondition = dataContext is ICondition;
                var isImage = dataContext is BitmapImage;

                if (isDesignElement || isDesign || isDataset || isDataTable || isImage || isCondition)
                {
                    OpenItem(dataContext);
                }
            }
        }

        private void TreeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            var treeItem = sender as TreeViewItem;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(treeItem, treeItem.DataContext, DragDropEffects.Move | DragDropEffects.None);
            }
        }
        private void RefreshTreeView()
        {            
            var dict = PopulateDictionaryWithExpanded(tvElementView.Items);
            tvElementView.Items.Clear();
            if (CurrentProject == null)
            {
                tvElementView.Visibility = System.Windows.Visibility.Hidden;
                return;
            }
            else
            {
                tvElementView.Visibility = System.Windows.Visibility.Visible;
            }
            var projectItem = CreateTreeViewItem(CurrentProject.Name, CurrentProject, true, dict);
            var designManager = CurrentProject.DesignManager;
            var designsItem = CreateTreeViewItem("Designs", designManager, true, dict);            
            var datasetItem = CreateTreeViewItem("Dataset", designManager.Dataset, true, dict);

            foreach (IDesign design in designManager.Designs)
            {
                var designItem = CreateTreeViewItem(design.Name, design, true, dict);
                var templateItem = CreateTreeViewItem("Template", design, false);
                designItem.Items.Add(templateItem);
                foreach (IDesignElement designElem in design.DesignElements.OrderBy(elem => elem.Layer))
                {
                    var elementItem = CreateTreeViewItem(designElem.Name, designElem, true, dict);
                    var condItem = CreateTreeViewItem("Condition", designElem.Condition, false);
                    elementItem.Items.Add(condItem);
                    designItem.Items.Add(elementItem);
                }
                designsItem.Items.Add(designItem);
            }

            foreach (DataTable dt in designManager.Dataset.Tables)
            {
                var tableItem = CreateTreeViewItem(dt.TableName, dt, true, dict);
                foreach (DataColumn dc in dt.Columns)
                {
                    var columnItem = CreateTreeViewItem(dc.ColumnName, dc, false);
                    tableItem.Items.Add(columnItem);
                }
                datasetItem.Items.Add(tableItem);
            }
            projectItem.Items.Add(designsItem);
            projectItem.Items.Add(datasetItem);
            tvElementView.Items.Add(projectItem);
        }

        private void tvElementView_AddDesign(object sender, RoutedEventArgs e)
        {
            var design = new Design("New Design", CurrentProject.DesignManager);
            CurrentProject.DesignManager.Designs.Add(design);
            RefreshTreeView();
        }

        private void tvElementView_AddDataset(object sender, RoutedEventArgs e)
        {
            if (CurrentProject.DesignManager.Dataset == null)
            {
                CurrentProject.DesignManager.Dataset = new DataSet("New Dataset");
                RefreshTreeView();
            }
        }
        #endregion

        #region Menu Events
        private void mnuMain_SaveProject(object sender, RoutedEventArgs e)
        {
            IO.ProjectIOManager.SaveProject(CurrentProject);            
        }

        private void mnuMain_LoadProject(object sender, RoutedEventArgs e)
        {
            var ofd = IO.ProjectIOManager.GetOpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                var project = IO.ProjectIOManager.LoadProject(ofd.FileName);
                CurrentProject = project;
                project.ProjectFilePath = ofd.FileName;
                project.Saved += project_Saved;
                RefreshTreeView();
                SetCurrentValue(ProjectLoaded, true);
            }
        }

        void project_Saved(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();
        }

        private void mnuMain_ExitApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuMain_CloseProject(object sender, RoutedEventArgs e)
        {
            if(CurrentProject == null)
            {
                return;
            }
            if (CurrentProject.IsDirty)
            {
                var dialogResult = MessageBox.Show("Would you like to save your current project?", "Save Before Closing?", MessageBoxButton.YesNoCancel);
                if (dialogResult == MessageBoxResult.Yes)
                {
                   CurrentProject.Save();
                }
                
            }
            CurrentProject = null;
            RefreshTreeView();
            tvElementView.Visibility = System.Windows.Visibility.Hidden;
            ccUserControl.Content = null;
            SetCurrentValue(ProjectLoaded, false);
        }

        private void mnuMain_CreateNewProject(object sender, RoutedEventArgs e)
        {
            var sfd = IO.ProjectIOManager.GetSaveFileDialog();            
            if (sfd.ShowDialog(this) == true)
            {
                var newProject = new Projects.GameProject(sfd.SafeFileName.Replace(".bgProj", ""));
                IO.ProjectIOManager.SaveProject(newProject, sfd.FileName);
                CurrentProject = newProject;
                CurrentProject.ProjectFilePath = sfd.FileName;
                RefreshTreeView();
                SetCurrentValue(ProjectLoaded, true);
            }
        }
        #endregion

        #region Context Menu
        private void tvElementView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //Clear current conext menu
            tvElementView.ContextMenu.Items.Clear();
            var contextMenu = tvElementView.ContextMenu;            
            var selectedItem = tvElementView.SelectedItem;
            if (selectedItem == null)
            {
                AddRefreshItem(contextMenu);
                return;
            }
            var dataContext = (selectedItem as FrameworkElement).DataContext;
            if (dataContext == null)
            {
                AddRefreshItem(contextMenu);
                return;
            }
            var isDesignElement = dataContext is IDesignElement;
            var isDesign = dataContext is IDesign;
            var isDataset = dataContext is DataSet;
            var isDataTable = dataContext is DataTable;
            var isDataColumn = dataContext is DataColumn;
            var isDesignManager = dataContext is IDesignManager;
            var isProject = dataContext is IProject;
            var isCondition = dataContext is ICondition;
            var isImage = dataContext is BitmapImage;

            if (isDesignManager)
            {
                AddNewDesignItem(contextMenu);
            }
            if (isDesign)
            {
                AddOpenItem(contextMenu);
                AddSetTemplateItem(contextMenu);
                AddNewImageElementItem(contextMenu);
                AddNewTextElementItem(contextMenu);
                AddDeleteItem(contextMenu);
            }
            if (isDesignElement)
            {
                AddOpenItem(contextMenu);
                AddDeleteItem(contextMenu);
                AddMoveItemUp(contextMenu);
                AddMoveItemDown(contextMenu);
            }
            if (isDataset)
            {
                AddOpenItem(contextMenu);
            }
            if (isDataTable)
            {
                AddOpenItem(contextMenu);
                AddDeleteItem(contextMenu);
            }
            if (isDataColumn)
            {
                AddDeleteItem(contextMenu);
            }
            if (isImage)
            {
                AddOpenItem(contextMenu);                
            }
            if (dataContext is INameable || isDataColumn || isDataTable)
            {
                AddRenameItem(contextMenu);
            }
            if (dataContext is IToggleable)
            {
                AddEnableDisableItem(contextMenu, (dataContext as IToggleable).Enabled);
            }
            AddRefreshItem(contextMenu);
        }
        #endregion

        #region Menu Item Command Creation
        private void AddMoveItemUp(ContextMenu menu)
        {
            var moveItem = new MenuItem() { Header = "Move Element Up" };
            moveItem.Click += tvElementView_MoveItemUp;
            menu.Items.Add(moveItem);
        }
        private void AddMoveItemDown(ContextMenu menu)
        {
            var moveItem = new MenuItem() { Header = "Move Element Down" };
            moveItem.Click += tvElementView_MoveItemDown;
            menu.Items.Add(moveItem);
        }

        private void AddEnableDisableItem(ContextMenu menu, bool currentState)
        {
            var enableItem = new MenuItem() { Header = currentState ? "Disable" : "Enable" };
            enableItem.Click += tvElementView_ToggleItem;
            menu.Items.Add(enableItem);
            menu.Items.Add(new Separator());

        }

        private void AddOpenItem(ContextMenu menu)
        {
            var openItem = new MenuItem() { Header = "Open" };
            openItem.Click += tvElementView_OpenItem;
            menu.Items.Add(openItem);
        }
        private void AddDeleteItem(ContextMenu menu)
        {
            var deleteItem = new MenuItem() { Header = "Delete" };
            deleteItem.Click += tvElementView_DeleteItem;
            menu.Items.Add(deleteItem);
        }
        private void AddNewDesignItem(ContextMenu menu)
        {
            var addNewDesign = new MenuItem() { Header = "Add New Design" };
            addNewDesign.Click += tvElementView_AddNewDesign;
            menu.Items.Add(addNewDesign);
        }
        private void AddNewTextElementItem(ContextMenu menu)
        {
            var addNewTextElement = new MenuItem() { Header = "Add New Text Element" };
            addNewTextElement.Click += tvElementView_AddNewTextDesignElement;
            menu.Items.Add(addNewTextElement);
        }
        private void AddNewImageElementItem(ContextMenu menu)
        {
            var addNewImageElement = new MenuItem() { Header = "Add New Image Element" };
            addNewImageElement.Click += tvElementView_AddNewImageDesignElement;
            menu.Items.Add(addNewImageElement);
        }

        private void AddSetTemplateItem(ContextMenu menu)
        {
            var setTemplate = new MenuItem() { Header = "Set Template" };
            setTemplate.Click += tvElementView_SetTemplate;
            menu.Items.Add(setTemplate);
        }
        private void AddRenameItem(ContextMenu menu)
        {
            var renameItem = new MenuItem() { Header = "Rename" };
            renameItem.Click += tvElementView_RenameItem;
            menu.Items.Add(renameItem);            
        }
        private void AddRefreshItem(ContextMenu menu)
        {
            if (menu.Items.Count > 0)
            {
                menu.Items.Add(new Separator());
            }
            var refreshCmd = new MenuItem() { Header = "Refresh (F5)" };
            refreshCmd.Click += refreshCmd_Click;
            menu.Items.Add(refreshCmd);
            
        }
        #endregion

        void refreshCmd_Click(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();
        }

        #region Context Menu Event Handlers

        private void tvElementView_ToggleItem(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
                return;
            if (selectedItem is IToggleable)
            {
                (selectedItem as IToggleable).Enabled = !(selectedItem as IToggleable).Enabled;
            }
        }
        private void tvElementView_RenameItem(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
                return;
            var siblingNames = new List<string>();
            if (selectedItem is IDesignElement)
            {
                var desElem = (selectedItem as IDesignElement);
                siblingNames.AddRange(desElem.Design.DesignElements.Where(el => el != desElem).Select(el => el.Name));
            }
            else if (selectedItem is IDesign)
            {
                var des = (selectedItem as IDesign);
                siblingNames.AddRange(des.DesignManager.Designs.Where(d => d != des).Select(el => el.Name));
            }
            if (selectedItem is INameable)
            {
                var renameWindow = new Input.RenameWindow(selectedItem as INameable, siblingNames);
                if (renameWindow.ShowDialog() == true)
                {
                    CurrentProject.IsDirty = true;
                    RefreshTreeView();
                }
            }
            else if (selectedItem is DataColumn)
            {
                var columns = (selectedItem as DataColumn).Table.Columns;
                foreach (DataColumn column in columns)
                {
                    if (column != selectedItem)
                    {
                        siblingNames.Add(column.ColumnName);
                    }
                } 
                var renameWindow = new Input.RenameWindow(selectedItem as DataColumn, siblingNames);
                if (renameWindow.ShowDialog() == true)
                {
                    CurrentProject.IsDirty = true;
                    RefreshTreeView();
                }
            }
            else if (selectedItem is DataTable)
            {
                var tables = (selectedItem as DataTable).DataSet.Tables;
                foreach (DataTable table in tables)
                {
                    if (table != selectedItem)
                    {
                        siblingNames.Add(table.TableName);
                    }
                }
                var renameWindow = new Input.RenameWindow(selectedItem as DataTable, siblingNames);
                if (renameWindow.ShowDialog() == true)
                {
                    CurrentProject.IsDirty = true;
                    RefreshTreeView();
                }
            }
        }
        private void tvElementView_MoveItemUp(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
                return;
            if (selectedItem is IDesignElement)
            {
                var designElem = selectedItem as IDesignElement;
                var design = designElem.Design;
                design.ReorderDesignElements(designElem, LayerMoveType.UP);
                RefreshTreeView();
            }
        }
        private void tvElementView_MoveItemDown(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
                return;
            if (selectedItem is IDesignElement)
            {
                var designElem = selectedItem as IDesignElement;
                var design = designElem.Design;
                design.ReorderDesignElements(designElem, LayerMoveType.DOWN);
                RefreshTreeView();
            }
        }
        private void tvElementView_DeleteItem(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
            {
                return;
            }
            if (!(selectedItem is IRemovable))
                return;
            if (MessageBox.Show("Are you sure you want to remove this item?", "Confirm Removal", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
            {
                    (selectedItem as IRemovable).Remove();
                RefreshTreeView();
            }
        }

        private void tvElementView_SetTemplate(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem == null)
                return;
            if (selectedItem is IDesign)
            {
                var templateEditor = new UserControls.ucTemplateEditor(selectedItem as IDesign);
                ccUserControl.Content = templateEditor;
                templateEditor.SetTemplate(selectedItem as IDesign);
            }
        }

        private void tvElementView_AddNewTextDesignElement(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem is IDesign)
            {
                var textElem = new TextDesignElement(selectedItem as IDesign);
                (selectedItem as IDesign).DesignElements.Add(textElem);
                (selectedItem as IDesign).DesignManager.Project.IsDirty = true;
            }
            RefreshTreeView();
        }
        private void tvElementView_AddNewImageDesignElement(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem is IDesign)
            {
                var ofd = IO.ProjectIOManager.GetImageFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    var sourceImg = new BitmapImage(new Uri(ofd.FileName));
                    var imgElem = new ImageDesignElement(selectedItem as IDesign, sourceImg);
                    (selectedItem as IDesign).DesignElements.Add(imgElem);
                    (selectedItem as IDesign).DesignManager.Project.IsDirty = true;
                }
            }
            RefreshTreeView();
        }
        private void tvElementView_AddNewDesign(object sender, RoutedEventArgs e)
        {
            var selectedItem = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            if (selectedItem is IDesignManager)
            {
                var newDesign = new Design(selectedItem as IDesignManager);
                (selectedItem as IDesignManager).Designs.Add(newDesign);
                (selectedItem as IDesignManager).Project.IsDirty = true;
            }
            RefreshTreeView();        
        }

        private void tvElementView_OpenItem(object sender, RoutedEventArgs e)
        {
            var item = (tvElementView.SelectedItem as FrameworkElement).DataContext;
            OpenItem(item);
        }
        #endregion

        private void SetUserControlProperties(UserControl uc)
        {
            uc.AllowDrop = true;
            ccUserControl.AllowDrop = true;
            ccUserControl.Content = uc;            
        }
        private void wndProjectManager_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentProject != null)
            {
                if (CurrentProject.IsDirty)
                {
                    var dialogResult = MessageBox.Show("Would you like to save your current project?", "Save Before Closing?", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        IO.ProjectIOManager.SaveProject(CurrentProject);
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        CurrentProject = null;
                        RefreshTreeView();
                    }
                    else if (dialogResult == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void CommandBinding_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshTreeView();
        }

        #region Drag and Drop
        private void ccUserControl_Drop(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            object item = null;
            if (dataObj.GetDataPresent(typeof(DataSet)))
            {
                item = dataObj.GetData(typeof(DataSet));
            }
            else if (dataObj.GetDataPresent(typeof(Design)))
            {
                item = dataObj.GetData(typeof(Design));
            }
            else if (dataObj.GetDataPresent(typeof(TextDesignElement)))
            {
                item = dataObj.GetData(typeof(TextDesignElement));
            }
            else if (dataObj.GetDataPresent(typeof(ImageDesignElement)))
            {
                item = dataObj.GetData(typeof(ImageDesignElement));
            }
            if (item != null)
            {
                OpenItem(item);
            }
            ccUserControl.AllowDrop = true;
        }
        private void OpenItemPreview(object item)
        {
            if (!(item is IDesign))
                return;
            var design = item as IDesign;
            var image = design.DrawImage();
            var ucContent = new UserControls.ucDesignEditor(design);
            ucContent.PreviewFirstItem();
            ccUserControl.Content = ucContent;
        }
        private void OpenItem(object item)
        {
            UserControl ucContent = null;
            if (item is ITextDesignElement)
            {
                ucContent = new UserControls.ucTextDesignElementEditor(item as ITextDesignElement);
            }
            else if (item is IImageDesignElement)
            {
                ucContent = new UserControls.ucImageDesignElementEditor(item as IImageDesignElement);
            }
            else if (item is IDesign)
            {
                ucContent = new UserControls.ucDesignEditor(item as IDesign);
            }
            else if (item is DataSet)
            {
                ucContent = new UserControls.ucDataSetEditor(item as DataSet, CurrentProject);
            }
            else if (item is ICondition)
            {
                ucContent = new UserControls.ucConditionEditor(item as ICondition);
            }
            else if (item is DataTable)
            {
                ucContent = new UserControls.ucDataSetEditor((item as DataTable).DataSet, CurrentProject);
                (ucContent as UserControls.ucDataSetEditor).cboTables.SelectedItem = (item as DataTable);
            }
            if (ucContent != null)
            {
                SetUserControlProperties(ucContent);
                CurrentProject.IsDirty = true;
            }
        }

        private void ccUserControl_DropOver(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            object item = null;
            if (dataObj.GetDataPresent(typeof(DataSet)))
            {
                item = dataObj.GetData(typeof(DataSet));
            }
            else if (dataObj.GetDataPresent(typeof(Design)))
            {
                item = dataObj.GetData(typeof(Design));
            }
            else if (dataObj.GetDataPresent(typeof(BitmapImage)))
            {
                item = dataObj.GetData(typeof(BitmapImage));
            }
            else if (dataObj.GetDataPresent(typeof(TextDesignElement)))
            {
                item = dataObj.GetData(typeof(TextDesignElement));
            }
            else if (dataObj.GetDataPresent(typeof(ImageDesignElement)))
            {
                item = dataObj.GetData(typeof(ImageDesignElement));
            }
            if (item == null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
        }
        #endregion

        #region Test Data
        private DataColumn GetTestColumn(int table, int column)
        {
            var col = new DataColumn();
            col.DataType = typeof(string);
            string colName = string.Empty;
            switch (table)
            {
                case 1:
                    switch (column)
                    {
                        case 0:
                            colName = "Casting Cost";
                            break;
                        case 1:
                            colName = "Card Type";
                            break;
                        case 2:
                            colName = "Illustrator";
                            break;
                        default:
                            colName = "Number of Widgets";
                            break;
                    }
                    break;
                case 0:
                default:
                    switch (column)
                    {
                        case 0:
                            colName = "Casting Cost";
                            break;
                        case 1:
                            colName = "Card Type";
                            break;
                        case 2:
                            colName = "Illustrator";
                            break;
                        default:
                            colName = "Number of Widgets";
                            break;
                    }
                    break;
            }
            col.ColumnName = colName;
            return col;
        }
        private void mnuMain_CreateSampleProject(object sender, RoutedEventArgs e)
        {
            var proj = new Projects.GameProject("TestData");
            proj.DesignManager = new Designs.DesignManager(proj);
            var testds = new DataSet("Test Dataset Fo Shizzle");
            var testTable1 = new DataTable("This is my table yo");
            var testTable2 = new DataTable("This is my other table yo");
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        var dc = GetTestColumn(0, k);
                        var dc2 = GetTestColumn(1, k);
                        testTable1.Columns.Add(dc);
                        testTable2.Columns.Add(dc2);
                    }
                }
                var drow1 = testTable1.NewRow();
                var drow2 = testTable2.NewRow();
                for (int k = 0; k < 3; k++)
                {
                    drow1[k] = i * k;
                    drow2[k] = i + k;
                }
                testTable1.Rows.Add(drow1);
                testTable2.Rows.Add(drow2);
            }
            testds.Tables.Add(testTable1);
            testds.Tables.Add(testTable2);
            proj.DesignManager.Dataset = testds;
            var newDesign = new Design("Cards of Fury", proj.DesignManager);
            var template = new BitmapImage(new Uri(@"C:\Users\Alex\Desktop\AerisDeath.png"));
            newDesign.Template = template;
            var img = new BitmapImage(new Uri(@"C:\Users\Alex\Desktop\elf.png"));
            var imgDesign = new ImageDesignElement(newDesign, img, 500.0, 250.0);
            var textDesign = new TextDesignElement(newDesign);
            textDesign.Name = "Fireballs";
            textDesign.Layer = 1;
            textDesign.Condition = new SimpleCondition(textDesign, testds.Tables[0].Columns[0], Designs.Condition.ConditionalOperator.Equals, "Test");
            textDesign.DataSource = testTable1;
            textDesign.ValueSource = testTable1.Columns[1];
            textDesign.Color = Brushes.Black;
            textDesign.Font = new FontFamily("Times New Roman");
            //textDesign.Origin = new PointF(0f, 0f);
            textDesign.Weight = FontWeights.Normal;
            textDesign.Style = FontStyles.Normal;
            textDesign.Text = "OMFG FIREBALLS IN MY FACE";
            textDesign.FontSize = 96.0;

            imgDesign.Name = "PRETTY ELF LADY";
            imgDesign.Condition = new AndCondition(imgDesign, new OrCondition(imgDesign, new SimpleCondition(imgDesign, testds.Tables[1].Columns[1], Designs.Condition.ConditionalOperator.Equals, "Test"),
                                                                    new SimpleCondition(imgDesign, testds.Tables[1].Columns[2], Designs.Condition.ConditionalOperator.GreaterThan, 5)),
                                                   new SimpleCondition(imgDesign, testds.Tables[0].Columns[2], Designs.Condition.ConditionalOperator.LessThan, 3),
                                                   new AndCondition(imgDesign, new SimpleCondition(imgDesign, testds.Tables[1].Columns[0], Designs.Condition.ConditionalOperator.Equals, "Face"),
                                                                     new SimpleCondition(imgDesign, testds.Tables[0].Columns[0], Designs.Condition.ConditionalOperator.Equals, 0.0f)
                                                                   )
                                                   );
            imgDesign.DataSource = testTable2;
            imgDesign.Layer = 2;

            newDesign.DesignElements.Add(textDesign);
            newDesign.DesignElements.Add(imgDesign);
            proj.DesignManager.Designs.Add(newDesign);
            IO.ProjectIOManager.SaveProject(proj, @"C:\Wolfpack Studios\Board Game Designer\Projects\TestProjectOmg.bgProj");
        }
        #endregion
    }
}
