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
    public partial class EditFuelRefilling
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

        [Parameter]
        public int Tank { get; set; }

        protected override async Task OnInitializedAsync()
        {
            fuelRefilling = await RAZDENService.GetFuelRefillingByTank(tank:Tank);
        }
        protected bool errorVisible;
        protected DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling fuelRefilling;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await RAZDENService.UpdateFuelRefilling(tank:Tank, fuelRefilling);
                DialogService.Close(fuelRefilling);
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