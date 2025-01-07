

namespace LW13._1
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResearcherId;
        private int _editProjId;
        private int _editExId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => listView.ItemsSource = await _dbService.GetResearchers());
            Task.Run(async () => listView1.ItemsSource = await _dbService.GetProjects());
            Task.Run(async () => listView2.ItemsSource = await _dbService.GetExperiments());
        }

        private async void saveButton_ClickedRes(object sender, EventArgs e)
        {
            if (_editResearcherId == 0) {
                await _dbService.CreateRes(new Researcher
                {
                    FirstName = FirstNameEntryField.Text,
                    Department = DepartmentEntryField.Text,
                    Position = PositionEntryField.Text,
                });
            }
            else
            {
                await _dbService.UpdateRes(new Researcher
                {
                    Id = _editResearcherId,
                    FirstName = FirstNameEntryField.Text,
                    Department = DepartmentEntryField.Text,
                    Position = PositionEntryField.Text,
                });

                _editResearcherId = 0;
            }
            FirstNameEntryField.Text = string.Empty;
            DepartmentEntryField.Text = string.Empty;
            PositionEntryField.Text = string.Empty;

            listView.ItemsSource = await _dbService.GetResearchers();
        }

        private async void listView_ItemTappedRes(object sender, ItemTappedEventArgs e)
        {
            var researcher = (Researcher)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResearcherId = researcher.Id;
                    FirstNameEntryField.Text = researcher.FirstName;
                    DepartmentEntryField.Text = researcher.Department;
                    PositionEntryField.Text = researcher.Position;
                    break;
                case "Delete":
                    await _dbService.DeleteRes(researcher);
                    listView.ItemsSource = await _dbService.GetResearchers();
                    break;
            }
        }

        private async void saveButton_ClickedProj(object sender, EventArgs e)
        {
            if (_editProjId == 0)
            {
                await _dbService.CreateProj(new Project
                {
                    ProjectName = ProjectNameEntryField.Text,
                    Description = DescriptionEntryField.Text,
                });
            }
            else
            {
                await _dbService.UpdateProj(new Project
                {
                    Id = _editProjId,
                    ProjectName = ProjectNameEntryField.Text,
                    Description = DescriptionEntryField.Text,
                });

                _editProjId = 0;
            }
            ProjectNameEntryField.Text = string.Empty;
            DescriptionEntryField.Text = string.Empty;

            listView1.ItemsSource = await _dbService.GetProjects();
        }

        private async void listView_ItemTappedProj(object sender, ItemTappedEventArgs e)
        {
            var project = (Project)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editProjId = project.Id;
                    ProjectNameEntryField.Text = project.ProjectName;
                    DescriptionEntryField.Text = project.Description;
                    break;
                case "Delete":
                    await _dbService.DeleteProj(project);
                    listView1.ItemsSource = await _dbService.GetProjects();
                    break;
            }
        }

        private async void saveButton_ClickedEx(object sender, EventArgs e)
        {
            if (_editExId == 0)
            {
                await _dbService.CreateEx(new Experiment
                {
                    ProjectName = ProjectNameEntryField_ex.Text,
                    ResearcherName = ResearcherNameEntryField.Text,
                    ExperimentName = ExperimentNameEntryField.Text,
                });
            }
            else
            {
                await _dbService.UpdateEx(new Experiment
                {
                    Id = _editExId,
                    ProjectName = ProjectNameEntryField_ex.Text,
                    ResearcherName = ResearcherNameEntryField.Text,
                    ExperimentName = ExperimentNameEntryField.Text,
                });

                _editExId = 0;
            }
            ProjectNameEntryField_ex.Text = string.Empty;
            ResearcherNameEntryField.Text = string.Empty;
            ExperimentNameEntryField.Text = string.Empty;

            listView2.ItemsSource = await _dbService.GetExperiments();
        }

        private async void listView_ItemTappedEx(object sender, ItemTappedEventArgs e)
        {
            var experiment = (Experiment)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editExId = experiment.Id;
                    ProjectNameEntryField_ex.Text= experiment.ProjectName;
                    ResearcherNameEntryField.Text = experiment.ResearcherName;
                    ExperimentNameEntryField.Text = experiment.ExperimentName;
                    break;
                case "Delete":
                    await _dbService.DeleteEx(experiment);
                    listView2.ItemsSource = await _dbService.GetExperiments();
                    break;
            }
        }

    }

}
