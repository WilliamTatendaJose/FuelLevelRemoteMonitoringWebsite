using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DeanRemoteMonitoringWeb.Client.Pages
{
    public partial class AddFuelTank
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public RAZDENService RAZDENService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            fuelTank = new DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank();
        }
        protected bool errorVisible;
        protected DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank fuelTank;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await RAZDENService.CreateFuelTank(fuelTank);
                DialogService.Close(fuelTank);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}