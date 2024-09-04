function AssignMetadataExtension(wrapper) {
	this._dashboarControl = wrapper.GetDashboardControl();
	this._performRequest = wrapper.PerformDataCallback.bind(wrapper);
	this.name = "dxdde-assign-metadata";
	this._menuItem = {
		id: this.name,
		title: "Assign metadata",
		click: this.assignMetadata.bind(this),
		selected: ko.observable(false),
		disabled: ko.computed(function () { return !this._dashboarControl.dashboard(); }, this),
		index: 113,
		hasSeparator: true,
		data: this,
	};
}

AssignMetadataExtension.prototype.assignMetadata = function () {
	if (this._toolbox) {
			// Will maybe be usefull later 09.04.2024 Janko Jovičić  - DevExpress.ui.notify("Hello");
			var dashboardid = this._dashboarControl.getDashboardId();
			var param = JSON.stringify({ DashboardID: dashboardid, ExtensionName: this.name });
			AssignMetadata(dashboardid);
			this._toolbox.menuVisible(false);

			
	}
}

AssignMetadataExtension.prototype.start = function () {
	this._toolbox = this._dashboarControl.findExtension('toolbox');
	this._toolbox && this._toolbox.menuItems.push(this._menuItem);
};

AssignMetadataExtension.prototype.stop = function () {
	this._toolbox && this._toolbox.menuItems.remove(this._menuItem);
};