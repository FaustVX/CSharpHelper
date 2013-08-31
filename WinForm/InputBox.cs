using System.Windows.Forms;

namespace CSharpHelper.WinForm
{
	public partial class InputBox : Form
	{
		public InputBox()
		{
			InitializeComponent();
		}

		public string Title
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		public override string Text
		{
			get { return textBox1 != null ? textBox1.Text : string.Empty; }
			set { textBox1.Text = value; }
		}

		private void Btn_Ok_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		public new DialogResult ShowDialog()
		{
			Text = string.Empty;
			return base.ShowDialog();
		}

		public new DialogResult ShowDialog(IWin32Window owner)
		{
			Text = string.Empty;
			return base.ShowDialog(owner);
		}

		public DialogResult ShowDialog(string title)
		{
			Title = title;
			return ShowDialog();
		}

		public DialogResult ShowDialog(string title, IWin32Window owner)
		{
			Title = title;
			return ShowDialog(owner);
		}
	}
}
