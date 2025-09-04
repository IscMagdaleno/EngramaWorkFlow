using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.TestModul.Utiles;
using WorkFlow.PWA.Shared.Common;

namespace WorkFlow.PWA.Areas.TestModul.Components
{
	public partial class TableTestTable : EngramaComponent
	{

		[Parameter] public DataTest Data { get; set; }

		protected override void OnInitialized()
		{

		}

		protected override async Task OnInitializedAsync()
		{
			Loading.Show();
			ShowSnake(await Data.PostTestTable());
			Loading.Hide();
		}





	}
}
