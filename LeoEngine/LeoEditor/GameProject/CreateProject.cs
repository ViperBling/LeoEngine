using LeoEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LeoEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        // 可以用来生成最初的Template.xml，生成一次后后面可以复用
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
        
        public byte[] Icon { get; set; }
        public byte[] ScreenShot { get; set; }
        public string IconFilePath { get; set; }
        public string ScreenShotFilePath { get; set; }
        public string ProjectFilePath { get; set; }
    }
    
    class CreateProject : ViewModeBase
    {
        // TODO: Get the path from the installation location
        private readonly string _templatePath = @"..\..\LeoEditor\ProjectTemplates";
        // 默认名称和路径
        private string _projectName = "NewProject";
        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\LeoProject\";
        // 定义可保存的模板集合
        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
        
        // 用于配合UI显示对应的字符
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }
        
        public CreateProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            try
            {
                // 读取所有Template中的内容，并读入对应的图标到template中，然后组合成完成的结构体
                var templatesFiles = Directory.GetFiles(_templatePath, "Template.xml", SearchOption.AllDirectories);
                Debug.Assert(templatesFiles.Any());
                foreach (var file in templatesFiles)
                {
                    var template = Serializer.FromeFile<ProjectTemplate>(file);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ScreenShotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "ScreenShot.png"));
                    template.ScreenShot = File.ReadAllBytes(template.ScreenShotFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));

                    _projectTemplates.Add(template);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: Log error;
            }
        }
    }
}