namespace MedSysProject.Models
{
    public class CProjectWarp
    {
        private Project _project = null;
        public CProjectWarp()
        {
            _project = new Project();
        }

        public Project project
        {
            get { return _project; }
            set { _project = value; }

        }
    }
}
