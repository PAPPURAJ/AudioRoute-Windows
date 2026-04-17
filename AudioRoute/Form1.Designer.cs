namespace AudioRoute;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Timer deviceRefreshTimer;
    private Label titleLabel;
    private Label subtitleLabel;
    private Panel selectionCard;
    private Panel controlCard;
    private Panel volumeCard;
    private Label sourceLabel;
    private ComboBox comboSource;
    private Label targetsLabel;
    private CheckedListBox checkedTargets;
    private Label latencyLabel;
    private NumericUpDown latencyInput;
    private CheckBox autoRefreshCheckBox;
    private Label refreshInfoLabel;
    private Button buttonStart;
    private Button buttonStop;
    private Button buttonRefresh;
    private Button buttonOpenMixer;
    private Label helpTitleLabel;
    private Label helpBodyLabel;
    private Label volumeTitleLabel;
    private FlowLayoutPanel targetVolumePanel;
    private Label statusCaptionLabel;
    private Label statusLabel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        deviceRefreshTimer = new System.Windows.Forms.Timer(components);
        titleLabel = new Label();
        subtitleLabel = new Label();
        selectionCard = new Panel();
        sourceLabel = new Label();
        comboSource = new ComboBox();
        targetsLabel = new Label();
        checkedTargets = new CheckedListBox();
        controlCard = new Panel();
        latencyLabel = new Label();
        latencyInput = new NumericUpDown();
        autoRefreshCheckBox = new CheckBox();
        refreshInfoLabel = new Label();
        buttonStart = new Button();
        buttonStop = new Button();
        buttonRefresh = new Button();
        buttonOpenMixer = new Button();
        helpTitleLabel = new Label();
        helpBodyLabel = new Label();
        volumeCard = new Panel();
        volumeTitleLabel = new Label();
        targetVolumePanel = new FlowLayoutPanel();
        statusCaptionLabel = new Label();
        statusLabel = new Label();
        selectionCard.SuspendLayout();
        controlCard.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)latencyInput).BeginInit();
        volumeCard.SuspendLayout();
        SuspendLayout();
        // 
        // deviceRefreshTimer
        // 
        deviceRefreshTimer.Interval = 3000;
        deviceRefreshTimer.Tick += deviceRefreshTimer_Tick;
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
        titleLabel.Location = new Point(32, 28);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(358, 32);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Play one movie on many headsets";
        // 
        // subtitleLabel
        // 
        subtitleLabel.AutoSize = true;
        subtitleLabel.Location = new Point(35, 72);
        subtitleLabel.MaximumSize = new Size(990, 0);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(938, 38);
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "AudioRoute captures the movie audio playing on one Windows output device and replays it to extra speakers or headphones. It runs by itself, but mirrored devices can still have more delay than the original device.";
        // 
        // selectionCard
        // 
        selectionCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        selectionCard.BackColor = Color.White;
        selectionCard.Controls.Add(sourceLabel);
        selectionCard.Controls.Add(comboSource);
        selectionCard.Controls.Add(targetsLabel);
        selectionCard.Controls.Add(checkedTargets);
        selectionCard.Location = new Point(35, 130);
        selectionCard.Name = "selectionCard";
        selectionCard.Size = new Size(470, 470);
        selectionCard.TabIndex = 2;
        // 
        // sourceLabel
        // 
        sourceLabel.AutoSize = true;
        sourceLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        sourceLabel.Location = new Point(24, 24);
        sourceLabel.Name = "sourceLabel";
        sourceLabel.Size = new Size(233, 19);
        sourceLabel.TabIndex = 0;
        sourceLabel.Text = "Source device already playing audio";
        // 
        // comboSource
        // 
        comboSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        comboSource.DropDownStyle = ComboBoxStyle.DropDownList;
        comboSource.FormattingEnabled = true;
        comboSource.Location = new Point(24, 52);
        comboSource.Name = "comboSource";
        comboSource.Size = new Size(422, 25);
        comboSource.TabIndex = 1;
        comboSource.SelectedIndexChanged += comboSource_SelectedIndexChanged;
        // 
        // targetsLabel
        // 
        targetsLabel.AutoSize = true;
        targetsLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        targetsLabel.Location = new Point(24, 102);
        targetsLabel.Name = "targetsLabel";
        targetsLabel.Size = new Size(172, 19);
        targetsLabel.TabIndex = 2;
        targetsLabel.Text = "Extra devices to mirror to";
        // 
        // checkedTargets
        // 
        checkedTargets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        checkedTargets.BackColor = Color.FromArgb(250, 252, 255);
        checkedTargets.BorderStyle = BorderStyle.FixedSingle;
        checkedTargets.CheckOnClick = true;
        checkedTargets.FormattingEnabled = true;
        checkedTargets.IntegralHeight = false;
        checkedTargets.Location = new Point(24, 130);
        checkedTargets.Name = "checkedTargets";
        checkedTargets.Size = new Size(422, 306);
        checkedTargets.TabIndex = 3;
        checkedTargets.ItemCheck += checkedTargets_ItemCheck;
        // 
        // controlCard
        // 
        controlCard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        controlCard.BackColor = Color.FromArgb(18, 35, 61);
        controlCard.Controls.Add(latencyLabel);
        controlCard.Controls.Add(latencyInput);
        controlCard.Controls.Add(autoRefreshCheckBox);
        controlCard.Controls.Add(refreshInfoLabel);
        controlCard.Controls.Add(buttonStart);
        controlCard.Controls.Add(buttonStop);
        controlCard.Controls.Add(buttonRefresh);
        controlCard.Controls.Add(buttonOpenMixer);
        controlCard.Controls.Add(helpTitleLabel);
        controlCard.Controls.Add(helpBodyLabel);
        controlCard.Location = new Point(533, 130);
        controlCard.Name = "controlCard";
        controlCard.Size = new Size(512, 260);
        controlCard.TabIndex = 3;
        // 
        // latencyLabel
        // 
        latencyLabel.AutoSize = true;
        latencyLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        latencyLabel.ForeColor = Color.White;
        latencyLabel.Location = new Point(24, 24);
        latencyLabel.Name = "latencyLabel";
        latencyLabel.Size = new Size(125, 19);
        latencyLabel.TabIndex = 0;
        latencyLabel.Text = "Latency buffer ms";
        // 
        // latencyInput
        // 
        latencyInput.Location = new Point(24, 52);
        latencyInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        latencyInput.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
        latencyInput.Name = "latencyInput";
        latencyInput.Size = new Size(120, 25);
        latencyInput.TabIndex = 1;
        latencyInput.Value = new decimal(new int[] { 250, 0, 0, 0 });
        // 
        // autoRefreshCheckBox
        // 
        autoRefreshCheckBox.AutoSize = true;
        autoRefreshCheckBox.Checked = true;
        autoRefreshCheckBox.CheckState = CheckState.Checked;
        autoRefreshCheckBox.ForeColor = Color.White;
        autoRefreshCheckBox.Location = new Point(24, 95);
        autoRefreshCheckBox.Name = "autoRefreshCheckBox";
        autoRefreshCheckBox.Size = new Size(176, 23);
        autoRefreshCheckBox.TabIndex = 2;
        autoRefreshCheckBox.Text = "Refresh devices every 3s";
        autoRefreshCheckBox.UseVisualStyleBackColor = true;
        autoRefreshCheckBox.CheckedChanged += autoRefreshCheckBox_CheckedChanged;
        // 
        // refreshInfoLabel
        // 
        refreshInfoLabel.AutoSize = true;
        refreshInfoLabel.ForeColor = Color.FromArgb(202, 214, 237);
        refreshInfoLabel.Location = new Point(24, 126);
        refreshInfoLabel.Name = "refreshInfoLabel";
        refreshInfoLabel.Size = new Size(126, 19);
        refreshInfoLabel.TabIndex = 3;
        refreshInfoLabel.Text = "Last scan: waiting";
        // 
        // buttonStart
        // 
        buttonStart.BackColor = Color.FromArgb(43, 167, 119);
        buttonStart.FlatStyle = FlatStyle.Flat;
        buttonStart.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        buttonStart.ForeColor = Color.White;
        buttonStart.Location = new Point(24, 163);
        buttonStart.Name = "buttonStart";
        buttonStart.Size = new Size(140, 40);
        buttonStart.TabIndex = 4;
        buttonStart.Text = "Start Routing";
        buttonStart.UseVisualStyleBackColor = false;
        buttonStart.Click += buttonStart_Click;
        // 
        // buttonStop
        // 
        buttonStop.BackColor = Color.FromArgb(230, 72, 72);
        buttonStop.FlatStyle = FlatStyle.Flat;
        buttonStop.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        buttonStop.ForeColor = Color.White;
        buttonStop.Location = new Point(178, 163);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new Size(120, 40);
        buttonStop.TabIndex = 5;
        buttonStop.Text = "Stop";
        buttonStop.UseVisualStyleBackColor = false;
        buttonStop.Click += buttonStop_Click;
        // 
        // buttonRefresh
        // 
        buttonRefresh.BackColor = Color.White;
        buttonRefresh.FlatStyle = FlatStyle.Flat;
        buttonRefresh.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        buttonRefresh.Location = new Point(312, 163);
        buttonRefresh.Name = "buttonRefresh";
        buttonRefresh.Size = new Size(176, 40);
        buttonRefresh.TabIndex = 6;
        buttonRefresh.Text = "Refresh Devices";
        buttonRefresh.UseVisualStyleBackColor = false;
        buttonRefresh.Click += buttonRefresh_Click;
        // 
        // buttonOpenMixer
        // 
        buttonOpenMixer.BackColor = Color.FromArgb(233, 240, 255);
        buttonOpenMixer.FlatStyle = FlatStyle.Flat;
        buttonOpenMixer.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
        buttonOpenMixer.ForeColor = Color.FromArgb(18, 35, 61);
        buttonOpenMixer.Location = new Point(24, 215);
        buttonOpenMixer.Name = "buttonOpenMixer";
        buttonOpenMixer.Size = new Size(180, 32);
        buttonOpenMixer.TabIndex = 7;
        buttonOpenMixer.Text = "Open App Volume Mixer";
        buttonOpenMixer.UseVisualStyleBackColor = false;
        buttonOpenMixer.Click += buttonOpenMixer_Click;
        // 
        // helpTitleLabel
        // 
        helpTitleLabel.AutoSize = true;
        helpTitleLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        helpTitleLabel.ForeColor = Color.White;
        helpTitleLabel.Location = new Point(260, 24);
        helpTitleLabel.Name = "helpTitleLabel";
        helpTitleLabel.Size = new Size(135, 19);
        helpTitleLabel.TabIndex = 8;
        helpTitleLabel.Text = "Standalone Limits";
        // 
        // helpBodyLabel
        // 
        helpBodyLabel.AutoSize = true;
        helpBodyLabel.ForeColor = Color.FromArgb(214, 223, 237);
        helpBodyLabel.Location = new Point(260, 52);
        helpBodyLabel.MaximumSize = new Size(225, 0);
        helpBodyLabel.Name = "helpBodyLabel";
        helpBodyLabel.Size = new Size(220, 133);
        helpBodyLabel.TabIndex = 9;
        helpBodyLabel.Text = "1. Set your movie to the source device.\r\n2. Select extra output devices here.\r\n3. Start with 200-350 ms for Bluetooth.\r\n\r\nThis app copies audio after Windows already played the source device, so mirrored devices can be late.";
        // 
        // volumeCard
        // 
        volumeCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        volumeCard.BackColor = Color.White;
        volumeCard.Controls.Add(volumeTitleLabel);
        volumeCard.Controls.Add(targetVolumePanel);
        volumeCard.Location = new Point(533, 410);
        volumeCard.Name = "volumeCard";
        volumeCard.Size = new Size(512, 190);
        volumeCard.TabIndex = 4;
        // 
        // volumeTitleLabel
        // 
        volumeTitleLabel.AutoSize = true;
        volumeTitleLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        volumeTitleLabel.Location = new Point(24, 24);
        volumeTitleLabel.Name = "volumeTitleLabel";
        volumeTitleLabel.Size = new Size(169, 19);
        volumeTitleLabel.TabIndex = 0;
        volumeTitleLabel.Text = "Per-device volume control";
        // 
        // targetVolumePanel
        // 
        targetVolumePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        targetVolumePanel.AutoScroll = true;
        targetVolumePanel.FlowDirection = FlowDirection.TopDown;
        targetVolumePanel.Location = new Point(24, 56);
        targetVolumePanel.Name = "targetVolumePanel";
        targetVolumePanel.Size = new Size(464, 114);
        targetVolumePanel.TabIndex = 1;
        targetVolumePanel.WrapContents = false;
        // 
        // statusCaptionLabel
        // 
        statusCaptionLabel.AutoSize = true;
        statusCaptionLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        statusCaptionLabel.Location = new Point(35, 620);
        statusCaptionLabel.Name = "statusCaptionLabel";
        statusCaptionLabel.Size = new Size(49, 19);
        statusCaptionLabel.TabIndex = 5;
        statusCaptionLabel.Text = "Status";
        // 
        // statusLabel
        // 
        statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        statusLabel.Location = new Point(35, 645);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(1010, 23);
        statusLabel.TabIndex = 6;
        statusLabel.Text = "Ready.";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(242, 245, 250);
        ClientSize = new Size(1080, 680);
        Controls.Add(statusLabel);
        Controls.Add(statusCaptionLabel);
        Controls.Add(volumeCard);
        Controls.Add(controlCard);
        Controls.Add(selectionCard);
        Controls.Add(subtitleLabel);
        Controls.Add(titleLabel);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(22, 28, 45);
        Name = "Form1";
        Text = "AudioRoute";
        selectionCard.ResumeLayout(false);
        selectionCard.PerformLayout();
        controlCard.ResumeLayout(false);
        controlCard.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)latencyInput).EndInit();
        volumeCard.ResumeLayout(false);
        volumeCard.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
