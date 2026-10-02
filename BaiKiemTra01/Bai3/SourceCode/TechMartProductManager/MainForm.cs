using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace TechMartProductManager;

public class MainForm : Form
{
    private readonly BindingList<Product> products = new();
    private readonly BindingSource bindingSource = new();
    private readonly ErrorProvider errorProvider = new();
    private readonly TableLayoutPanel mainLayout = new();
    private readonly TextBox txtProductId = new();
    private readonly TextBox txtProductName = new();
    private readonly TextBox txtUnitPrice = new();
    private readonly TextBox txtQuantity = new();
    private readonly TextBox txtSearch = new();
    private readonly ComboBox cboCategory = new();
    private readonly PictureBox picAvatar = new();
    private readonly DataGridView dgvProducts = new();
    private readonly ToolStripStatusLabel statusLabel = new();
    private int nextId = 1;
    private bool loadingFields;
    private Product? selectedProduct;
    private readonly BindingList<Product> visibleProducts = new();

    public MainForm()
    {
        Text = "TechMart Product Manager";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);
        Size = new Size(1250, 760);
        Font = new Font("Segoe UI", 9F);
        BuildInterface();
        ConfigureGrid();
        bindingSource.DataSource = visibleProducts;
        dgvProducts.DataSource = bindingSource;
        UpdateStatus();
        errorProvider.ContainerControl = this;
    }

    private void BuildInterface()
    {
        var menu = new MenuStrip();
        var fileMenu = new ToolStripMenuItem("&File");
        var exportItem = new ToolStripMenuItem("Export &CSV", null, (_, _) => ExportCsv()) { ShortcutKeys = Keys.Control | Keys.E };
        var exitItem = new ToolStripMenuItem("E&xit", null, (_, _) => Close()) { ShortcutKeys = Keys.Control | Keys.X };
        fileMenu.DropDownItems.Add(exportItem);
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add(exitItem);
        menu.Items.Add(fileMenu);
        MainMenuStrip = menu;

        var status = new StatusStrip();
        statusLabel.Text = "Tổng số sản phẩm: 0";
        status.Items.Add(statusLabel);

        mainLayout.Dock = DockStyle.Fill;
        mainLayout.ColumnCount = 2;
        mainLayout.RowCount = 1;
        mainLayout.Padding = new Padding(10);
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

        var leftGroup = new GroupBox { Text = "Thông tin sản phẩm", Dock = DockStyle.Fill, Padding = new Padding(10) };
        var rightGroup = new GroupBox { Text = "Danh sách sản phẩm", Dock = DockStyle.Fill, Padding = new Padding(10) };
        mainLayout.Controls.Add(leftGroup, 0, 0);
        mainLayout.Controls.Add(rightGroup, 1, 0);

        var leftLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 9,
            AutoScroll = true, Padding = new Padding(4)
        };
        leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
        leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
        for (int i = 0; i < 5; i++)
            leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 145));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        leftGroup.Controls.Add(leftLayout);

        ConfigureTextBox(txtProductId, "txtProductId", readOnly: true);
        ConfigureTextBox(txtProductName, "txtProductName");
        ConfigureTextBox(txtUnitPrice, "txtUnitPrice");
        ConfigureTextBox(txtQuantity, "txtQuantity");

        AddField(leftLayout, 0, "Mã SP:", txtProductId);
        AddField(leftLayout, 1, "Tên SP:", txtProductName);
        AddField(leftLayout, 2, "Đơn giá:", txtUnitPrice);
        AddField(leftLayout, 3, "Số lượng:", txtQuantity);

        cboCategory.Name = "cboCategory";
        cboCategory.Dock = DockStyle.Fill;
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.DisplayMember = "Text";
        cboCategory.ValueMember = "Value";
        cboCategory.DataSource = new List<CategoryOption>
        {
            new("Điện thoại", "Điện thoại"),
            new("Laptop", "Laptop"),
            new("Phụ kiện", "Phụ kiện")
        };
        AddField(leftLayout, 4, "Danh mục:", cboCategory);

        picAvatar.Name = "picAvatar";
        picAvatar.Dock = DockStyle.Fill;
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.BorderStyle = BorderStyle.FixedSingle;
        picAvatar.BackColor = Color.WhiteSmoke;
        leftLayout.Controls.Add(new Label { Text = "Ảnh:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 5);
        leftLayout.Controls.Add(picAvatar, 1, 5);

        var chooseImage = new Button { Name = "btnChooseImage", Text = "Chọn ảnh...", AutoSize = true, Anchor = AnchorStyles.Left };
        chooseImage.Click += (_, _) => ChooseImage();
        leftLayout.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 6);
        leftLayout.Controls.Add(chooseImage, 1, 6);

        var buttons = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, RowCount = 2, AutoSize = true };
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        var btnAdd = new Button { Name = "btnAdd", Text = "Thêm mới", Dock = DockStyle.Fill, Height = 34 };
        var btnUpdate = new Button { Name = "btnUpdate", Text = "Cập nhật", Dock = DockStyle.Fill, Height = 34 };
        var btnDelete = new Button { Name = "btnDelete", Text = "Xóa", Dock = DockStyle.Fill, Height = 34 };
        var btnClear = new Button { Name = "btnClear", Text = "Làm mới", Dock = DockStyle.Fill, Height = 34 };
        btnAdd.Click += (_, _) => AddProduct();
        btnUpdate.Click += (_, _) => UpdateProduct();
        btnDelete.Click += (_, _) => DeleteProduct();
        btnClear.Click += (_, _) => ClearFields();
        buttons.Controls.Add(btnAdd, 0, 0);
        buttons.Controls.Add(btnUpdate, 1, 0);
        buttons.Controls.Add(btnDelete, 0, 1);
        buttons.Controls.Add(btnClear, 1, 1);
        leftLayout.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 7);
        leftLayout.Controls.Add(buttons, 1, 7);

        var btnExportCsv = new Button { Name = "btnExportCsv", Text = "Xuất CSV", Dock = DockStyle.Top, Height = 34 };
        btnExportCsv.Click += (_, _) => ExportCsv();
        leftLayout.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 8);
        leftLayout.Controls.Add(btnExportCsv, 1, 8);

        var rightLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        rightLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rightGroup.Controls.Add(rightLayout);

        var searchLayout = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, RowCount = 1, AutoSize = true, Padding = new Padding(0, 0, 0, 8) };
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchLayout.Controls.Add(new Label { Text = "Tìm tên SP:", AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 5, 8, 0) }, 0, 0);
        txtSearch.Name = "txtSearch";
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.PlaceholderText = "Nhập tên sản phẩm để tìm...";
        txtSearch.TextChanged += (_, _) => ApplySearch();
        searchLayout.Controls.Add(txtSearch, 1, 0);
        rightLayout.Controls.Add(searchLayout, 0, 0);

        dgvProducts.Name = "dgvProducts";
        dgvProducts.Dock = DockStyle.Fill;
        dgvProducts.BackgroundColor = SystemColors.Window;
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.ReadOnly = true;
        dgvProducts.MultiSelect = false;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProducts.RowHeadersVisible = false;
        dgvProducts.SelectionChanged += (_, _) => LoadSelectedProduct();
        rightLayout.Controls.Add(dgvProducts, 0, 1);

        Controls.Add(mainLayout);
        Controls.Add(status);
        Controls.Add(menu);
        MainMenuStrip = menu;
    }

    private static void ConfigureTextBox(TextBox box, string name, bool readOnly = false)
    {
        box.Name = name;
        box.Dock = DockStyle.Fill;
        box.ReadOnly = readOnly;
    }

    private static void AddField(TableLayoutPanel layout, int row, string label, Control control)
    {
        layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 7, 3, 7) }, 0, row);
        control.Margin = new Padding(3, 4, 3, 4);
        layout.Controls.Add(control, 1, row);
    }

    private void ConfigureGrid()
    {
        dgvProducts.Columns.Clear();
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductId", HeaderText = "Mã SP", DataPropertyName = nameof(Product.ProductId), FillWeight = 55 });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductName", HeaderText = "Tên SP", DataPropertyName = nameof(Product.ProductName), FillWeight = 140 });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategory", HeaderText = "Danh mục", DataPropertyName = nameof(Product.Category), FillWeight = 90 });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colUnitPrice", HeaderText = "Đơn giá", DataPropertyName = nameof(Product.UnitPrice),
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 90
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQuantity", HeaderText = "Số lượng", DataPropertyName = nameof(Product.Quantity), FillWeight = 65 });
    }

    private bool ValidateFields()
    {
        errorProvider.Clear();
        bool valid = true;
        if (string.IsNullOrWhiteSpace(txtProductName.Text))
        {
            errorProvider.SetError(txtProductName, "Tên sản phẩm không được để trống.");
            valid = false;
        }
        if (!decimal.TryParse(txtUnitPrice.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal price) || price <= 0)
        {
            errorProvider.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0.");
            valid = false;
        }
        if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity < 0)
        {
            errorProvider.SetError(txtQuantity, "Số lượng phải là số nguyên lớn hơn hoặc bằng 0.");
            valid = false;
        }
        return valid;
    }

    private Product CreateProduct(string id) => new()
    {
        ProductId = id,
        ProductName = txtProductName.Text.Trim(),
        Category = cboCategory.SelectedValue?.ToString() ?? "Điện thoại",
        UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture),
        Quantity = int.Parse(txtQuantity.Text.Trim()),
        ImagePath = picAvatar.Tag as string
    };

    private void AddProduct()
    {
        if (!ValidateFields()) return;
        var product = CreateProduct($"SP{nextId++:000}");
        products.Add(product);
        ApplySearch();
        SelectProduct(product);
        UpdateStatus();
        MessageBox.Show("Đã thêm sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void UpdateProduct()
    {
        if (selectedProduct is null)
        {
            MessageBox.Show("Hãy chọn một sản phẩm trong bảng để cập nhật.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!ValidateFields()) return;

        selectedProduct.ProductName = txtProductName.Text.Trim();
        selectedProduct.Category = cboCategory.SelectedValue?.ToString() ?? "Điện thoại";
        selectedProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture);
        selectedProduct.Quantity = int.Parse(txtQuantity.Text.Trim());
        selectedProduct.ImagePath = picAvatar.Tag as string;
        RefreshVisibleProducts();
        SelectProduct(selectedProduct);
        UpdateStatus();
        MessageBox.Show("Đã cập nhật sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DeleteProduct()
    {
        if (selectedProduct is null)
        {
            MessageBox.Show("Hãy chọn sản phẩm cần xóa.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var answer = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm '{selectedProduct.ProductName}' không?",
            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes) return;
        products.Remove(selectedProduct);
        selectedProduct = null;
        ApplySearch();
        ClearFields();
        UpdateStatus();
    }

    private void ApplySearch()
    {
        string keyword = txtSearch.Text.Trim();
        visibleProducts.Clear();
        foreach (var product in products.Where(p => p.ProductName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)))
            visibleProducts.Add(product);
        bindingSource.ResetBindings(false);
        UpdateStatus();
    }

    private void RefreshVisibleProducts()
    {
        string keyword = txtSearch.Text.Trim();
        visibleProducts.Clear();
        foreach (var product in products.Where(p => p.ProductName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)))
            visibleProducts.Add(product);
        bindingSource.ResetBindings(false);
    }

    private void LoadSelectedProduct()
    {
        if (loadingFields || dgvProducts.CurrentRow?.DataBoundItem is not Product product) return;
        selectedProduct = product;
        loadingFields = true;
        txtProductId.Text = product.ProductId;
        txtProductName.Text = product.ProductName;
        txtUnitPrice.Text = product.UnitPrice.ToString("0.##", CultureInfo.CurrentCulture);
        txtQuantity.Text = product.Quantity.ToString();
        cboCategory.SelectedValue = product.Category;
        LoadProductImage(product.ImagePath);
        errorProvider.Clear();
        loadingFields = false;
    }

    private void SelectProduct(Product product)
    {
        int index = visibleProducts.IndexOf(product);
        if (index >= 0 && index < dgvProducts.Rows.Count)
        {
            dgvProducts.ClearSelection();
            dgvProducts.Rows[index].Selected = true;
            dgvProducts.CurrentCell = dgvProducts.Rows[index].Cells[0];
        }
    }

    private void ClearFields()
    {
        loadingFields = true;
        selectedProduct = null;
        txtProductId.Clear();
        txtProductName.Clear();
        txtUnitPrice.Clear();
        txtQuantity.Clear();
        if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
        picAvatar.Image = null;
        picAvatar.Tag = null;
        errorProvider.Clear();
        dgvProducts.ClearSelection();
        loadingFields = false;
        txtProductName.Focus();
    }

    private void ChooseImage()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Chọn ảnh sản phẩm",
            Filter = "Tệp ảnh|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Tất cả tệp|*.*"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        picAvatar.Tag = dialog.FileName;
        LoadProductImage(dialog.FileName);
    }

    private void LoadProductImage(string? path)
    {
        if (picAvatar.Image is not null)
        {
            var old = picAvatar.Image;
            picAvatar.Image = null;
            old.Dispose();
        }
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var temp = Image.FromStream(stream);
                picAvatar.Image = new Bitmap(temp);
                picAvatar.Tag = path;
            }
            catch
            {
                picAvatar.Image = null;
            }
        }
        else
        {
            picAvatar.Image = null;
        }
    }

    private void ExportCsv()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Xuất danh sách sản phẩm",
            Filter = "CSV (*.csv)|*.csv",
            FileName = "TechMartProducts.csv",
            DefaultExt = "csv",
            AddExtension = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        static string Csv(string? value) => "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";
        var sb = new StringBuilder();
        sb.AppendLine("Mã SP,Tên SP,Danh mục,Đơn giá,Số lượng");
        foreach (var p in products)
            sb.AppendLine(string.Join(",", Csv(p.ProductId), Csv(p.ProductName), Csv(p.Category),
                Csv(p.UnitPrice.ToString("0", CultureInfo.InvariantCulture)), Csv(p.Quantity.ToString(CultureInfo.InvariantCulture))));
        File.WriteAllText(dialog.FileName, sb.ToString(), new UTF8Encoding(true));
        MessageBox.Show("Xuất CSV thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void UpdateStatus() => statusLabel.Text = $"Tổng số sản phẩm: {products.Count}";

    private sealed record CategoryOption(string Text, string Value);
}
