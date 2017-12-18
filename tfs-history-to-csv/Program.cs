using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tfs_history_to_csv
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.Out.WriteLine("(c) @Wietze, 2017");

            // Let user select project
            TeamProjectPicker tpp = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, true) { Text = "Select your TFS Project" };
            if (tpp.ShowDialog() != DialogResult.OK) return;

            // Get TFS information
            Console.Out.WriteLine("Obtaining information...");
            VersionControlServer VersionControl = tpp.SelectedTeamProjectCollection.GetService<VersionControlServer>();
            string path = VersionControl.GetTeamProject(tpp.SelectedProjects[0].Name).ServerItem;
            var query_history = VersionControl.QueryHistory(path, VersionSpec.Latest, 0, RecursionType.Full, null, new ChangesetVersionSpec(1), VersionSpec.Latest, Int32.MaxValue, true, true, true, true);

            // Iterate over TFS data
            Console.Out.WriteLine("Generating CSV...");
            StringBuilder output = new StringBuilder();
            foreach (Changeset cs in query_history)
            {
                var id = cs.ChangesetId;
                var date = cs.CreationDate;
                var user = cs.Owner;
                var comment = cs.Comment;
                var changes = cs.Changes.Select(x => new Tuple<string, string>(x.ChangeType.ToString(), x.Item.ServerItem));
                foreach (var change in changes)
                    output.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"" + Environment.NewLine, id, date.ToString("o", System.Globalization.CultureInfo.InvariantCulture), date, user, comment.Replace("\"", "\"\""), change.Item1, change.Item2.Replace("\"", "\"\""));
            }

            // Ask user to select output file
            SaveFileDialog sfd = new SaveFileDialog()
            {
                AutoUpgradeEnabled = true,
                FileName = tpp.SelectedProjects[0].Name,
                Filter = "CSV|*.csv",
                RestoreDirectory = true,
                Title = "Set output file"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            // Write to file
            Console.Out.WriteLine("Writing CSV...");
            using (var t = new System.IO.StreamWriter(sfd.FileName))
            {
                t.Write("ChangesetID,IsoDate,Date,User,Comment,Change,File" + Environment.NewLine);
                t.Write(output);
            }
        }
    }
}