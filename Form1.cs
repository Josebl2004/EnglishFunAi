using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EnglishFunAI
{
    public partial class MainForm : Form
    {
        private DatabaseService dbService;
        private int usuarioId = -1;
        private string nombreUsuario;
        public MainForm(string nombreUsuario)
        {
            this.nombreUsuario = nombreUsuario;
            InitializeComponent();
            // Inicializar conexión al cargar
            string connStr = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Database=EnglishFunAI;";
            dbService = new DatabaseService(connStr);
            usuarioId = dbService.RegistrarUsuario(nombreUsuario);
        }
        public MainForm() : this("") { }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // No se requiere inicialización de base de datos
        }

        // Eventos para abrir los formularios de cada módulo
        private void btnChat_Click(object sender, EventArgs e)
        {
            ChatForm chatForm = new ChatForm();
            chatForm.ShowDialog();
        }
        private void btnTranslation_Click(object sender, EventArgs e)
        {
            TranslationForm translationForm = new TranslationForm();
            translationForm.ShowDialog();
        }
        private void btnGames_Click(object sender, EventArgs e)
        {
            GamesForm gamesForm = new GamesForm();
            gamesForm.ShowDialog();
            dbService?.RegistrarProgreso(usuarioId, "Juegos", "Sesión de juegos iniciada");
        }
        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (dbService != null && usuarioId > 0)
            {
                dbService.RegistrarProgreso(usuarioId, "Historial", "Historial consultado");
                HistoryForm historyForm = new HistoryForm(dbService, usuarioId);
                historyForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay conexión a la base de datos o usuario no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnVocabulary_Click(object sender, EventArgs e)
        {
            VocabularyForm vocabularyForm = new VocabularyForm();
            vocabularyForm.ShowDialog();
        }
    }

    // Formulario para Chat IA
    public class ChatForm : Form
    {
        private TextBox txtChat;
        private TextBox txtInput;
        private Button btnSend;
        private OpenAIService openAIService;

        public ChatForm()
        {
            this.Text = "Chat IA";
            this.Size = new System.Drawing.Size(520, 600); // antes 400x500
            this.BackColor = System.Drawing.Color.FromArgb(255, 240, 248, 255); // Azul muy claro

            txtChat = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(340, 320),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                BackColor = System.Drawing.Color.WhiteSmoke
            };
            txtInput = new TextBox
            {
                Location = new System.Drawing.Point(20, 360),
                Size = new System.Drawing.Size(260, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                BackColor = System.Drawing.Color.White
            };
            btnSend = new Button
            {
                Text = "Enviar",
                Location = new System.Drawing.Point(290, 360),
                Size = new System.Drawing.Size(70, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightSkyBlue,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;

            this.Controls.Add(txtChat);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnSend);

            // Solicitar la API Key al usuario (puedes mejorar esto con un formulario seguro)
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Por favor, configura la variable de entorno OPENAI_API_KEY con tu clave de OpenAI.", "API Key requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                openAIService = null;
                this.Close();
                return;
            }
            openAIService = new OpenAIService(apiKey);
        }

#pragma warning disable CS8765
        // Reemplazar la firma de BtnSend_Click para que no use tipos anulables
        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (openAIService == null) return;
            string userMsg = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userMsg)) return;
            txtChat.AppendText($"Tú: {userMsg}\r\n");
            txtInput.Clear();
            btnSend.Enabled = false;
            try
            {
                string response = await openAIService.GetChatResponseAsync(userMsg);
                txtChat.AppendText($"EnglishFun AI: {response}\r\n");
            }
            catch (Exception ex)
            {
                txtChat.AppendText($"[Error]: {ex.Message}\r\n");
            }
            btnSend.Enabled = true;
        }
#pragma warning restore CS8765
    }

    // Formulario para Traducción
    public class TranslationForm : Form
    {
        private Label lblSource;
        private TextBox txtSource;
        private Label lblResult;
        private TextBox txtResult;
        private Button btnTranslateToEn;
        private Button btnTranslateToEs;
        private OpenAIService openAIService;

        public TranslationForm()
        {
            this.Text = "Traducción";
            this.Size = new System.Drawing.Size(520, 500); // antes 400x350
            this.BackColor = System.Drawing.Color.FromArgb(255, 235, 255, 235); // Verde pastel

            lblSource = new Label
            {
                Text = "Texto:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(60, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.SeaGreen
            };
            txtSource = new TextBox
            {
                Location = new System.Drawing.Point(90, 20),
                Size = new System.Drawing.Size(270, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                BackColor = System.Drawing.Color.White
            };
            btnTranslateToEn = new Button
            {
                Text = "A Inglés",
                Location = new System.Drawing.Point(90, 70),
                Size = new System.Drawing.Size(120, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightSkyBlue,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnTranslateToEn.FlatAppearance.BorderSize = 0;
            btnTranslateToEs = new Button
            {
                Text = "A Español",
                Location = new System.Drawing.Point(240, 70),
                Size = new System.Drawing.Size(120, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Plum,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnTranslateToEs.FlatAppearance.BorderSize = 0;
            lblResult = new Label
            {
                Text = "Resultado:",
                Location = new System.Drawing.Point(20, 130),
                Size = new System.Drawing.Size(90, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.SeaGreen
            };
            txtResult = new TextBox
            {
                Location = new System.Drawing.Point(20, 170),
                Size = new System.Drawing.Size(340, 80),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                Multiline = true,
                ReadOnly = true,
                BackColor = System.Drawing.Color.WhiteSmoke
            };
            btnTranslateToEn.Click += BtnTranslateToEn_Click;
            btnTranslateToEs.Click += BtnTranslateToEs_Click;

            this.Controls.Add(lblSource);
            this.Controls.Add(txtSource);
            this.Controls.Add(btnTranslateToEn);
            this.Controls.Add(btnTranslateToEs);
            this.Controls.Add(lblResult);
            this.Controls.Add(txtResult);

            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Por favor, configura la variable de entorno OPENAI_API_KEY con tu clave de OpenAI.", "API Key requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                openAIService = null;
                this.Close();
                return;
            }
            openAIService = new OpenAIService(apiKey);
        }

        private async void BtnTranslateToEn_Click(object sender, EventArgs e)
        {
            if (openAIService == null) return;
            string text = txtSource.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            btnTranslateToEn.Enabled = false;
            btnTranslateToEs.Enabled = false;
            try
            {
                string prompt = $"Traduce este texto al inglés para un niño: '{text}'";
                string result = await openAIService.GetChatResponseAsync(prompt);
                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                txtResult.Text = $"[Error]: {ex.Message}";
            }
            btnTranslateToEn.Enabled = true;
            btnTranslateToEs.Enabled = true;
        }

        private async void BtnTranslateToEs_Click(object sender, EventArgs e)
        {
            if (openAIService == null) return;
            string text = txtSource.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            btnTranslateToEn.Enabled = false;
            btnTranslateToEs.Enabled = false;
            try
            {
                string prompt = $"Traduce este texto al español para un niño: '{text}'";
                string result = await openAIService.GetChatResponseAsync(prompt);
                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                txtResult.Text = $"[Error]: {ex.Message}";
            }
            btnTranslateToEn.Enabled = true;
            btnTranslateToEs.Enabled = true;
        }
    }

    // Formulario para Juegos
    public class GamesForm : Form
    {
        public GamesForm()
        {
            this.Text = "Juegos";
            this.Size = new System.Drawing.Size(520, 600); // antes 400x400
            this.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 220); // Amarillo pastel

            var lbl = new Label
            {
                Text = "Elige un juego:",
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(300, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Goldenrod
            };
            var btnQuiz = new Button
            {
                Text = "Quiz de Vocabulario",
                Location = new System.Drawing.Point(50, 80),
                Size = new System.Drawing.Size(280, 60),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightSkyBlue,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnQuiz.FlatAppearance.BorderSize = 0;
            btnQuiz.Click += BtnQuiz_Click;

            var btnCompletar = new Button
            {
                Text = "Completa la Palabra",
                Location = new System.Drawing.Point(50, 160),
                Size = new System.Drawing.Size(280, 60),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightGreen,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnCompletar.FlatAppearance.BorderSize = 0;
            btnCompletar.Click += BtnCompletar_Click;

            var btnImagen = new Button
            {
                Text = "Adivina la Imagen",
                Location = new System.Drawing.Point(50, 240),
                Size = new System.Drawing.Size(280, 60),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Plum,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnImagen.FlatAppearance.BorderSize = 0;
            btnImagen.Click += BtnImagen_Click;

            this.Controls.Add(lbl);
            this.Controls.Add(btnQuiz);
            this.Controls.Add(btnCompletar);
            this.Controls.Add(btnImagen);
        }

        private void BtnQuiz_Click(object sender, EventArgs e)
        {
            var quiz = new QuizForm();
            quiz.ShowDialog();
        }

        private void BtnCompletar_Click(object sender, EventArgs e)
        {
            var completar = new CompletarPalabraForm();
            completar.ShowDialog();
        }

        private void BtnImagen_Click(object sender, EventArgs e)
        {
            var imagen = new ImagenJuegoForm();
            imagen.ShowDialog();
        }
    }

    // Juego: Quiz de Vocabulario
    public class QuizForm : Form
    {
        private Label lblPregunta;
        private Button[] opciones;
        private string[] palabras = { "cat", "dog", "apple", "house", "car" };
        private string[] traducciones = { "gato", "perro", "manzana", "casa", "auto" };
        private int preguntaActual = 0;
        private int puntaje = 0;

        public QuizForm()
        {
            this.Text = "Quiz de Vocabulario";
            this.Size = new System.Drawing.Size(520, 400); // antes 400x300
            this.BackColor = System.Drawing.Color.FromArgb(255, 240, 255, 255); // Celeste pastel

            lblPregunta = new Label
            {
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(320, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.MediumVioletRed
            };
            this.Controls.Add(lblPregunta);

            opciones = new Button[3];
            for (int i = 0; i < 3; i++)
            {
                opciones[i] = new Button
                {
                    Location = new System.Drawing.Point(50, 90 + i * 50),
                    Size = new System.Drawing.Size(280, 40),
                    Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                    BackColor = System.Drawing.Color.LightGreen,
                    ForeColor = System.Drawing.Color.DarkSlateBlue,
                    FlatStyle = FlatStyle.Flat
                };
                opciones[i].FlatAppearance.BorderSize = 0;
                opciones[i].Click += Opcion_Click;
                this.Controls.Add(opciones[i]);
            }
            MostrarPregunta();
        }

        private void MostrarPregunta()
        {
            if (preguntaActual >= palabras.Length)
            {
                MessageBox.Show($"¡Juego terminado! Puntaje: {puntaje}/{palabras.Length}", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            lblPregunta.Text = $"¿Qué significa '{palabras[preguntaActual]}'?";
            var respuestas = new string[3];
            respuestas[0] = traducciones[preguntaActual];
            var rand = new Random();
            int correcta = rand.Next(3);
            for (int i = 1; i < 3; i++)
            {
                do { respuestas[i] = traducciones[rand.Next(traducciones.Length)]; } while (respuestas[i] == respuestas[0] || (i == 2 && respuestas[2] == respuestas[1]));
            }
            // Mezclar respuestas
            for (int i = 0; i < 3; i++)
            {
                var temp = respuestas[i];
                int swap = rand.Next(3);
                respuestas[i] = respuestas[swap];
                respuestas[swap] = temp;
            }
            for (int i = 0; i < 3; i++)
                opciones[i].Text = respuestas[i];
        }

        private void Opcion_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Text == traducciones[preguntaActual])
                puntaje++;
            preguntaActual++;
            MostrarPregunta();
        }
    }

    // Formulario para Historial
    public class HistoryForm : Form
    {
        private DatabaseService dbService;
        private int usuarioId = -1;
        private ComboBox cmbUsuarios;
        private DataGridView grid;
        public HistoryForm(DatabaseService dbService, int usuarioId)
        {
            this.Text = "Historial";
            this.Size = new System.Drawing.Size(600, 500);
            this.BackColor = System.Drawing.Color.FromArgb(255, 240, 255, 240);
            this.dbService = dbService;
            this.usuarioId = usuarioId;
            var lbl = new Label
            {
                Text = "Selecciona un jugador:",
                Location = new System.Drawing.Point(20, 10),
                Size = new System.Drawing.Size(200, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.SeaGreen
            };
            this.Controls.Add(lbl);
            cmbUsuarios = new ComboBox
            {
                Location = new System.Drawing.Point(220, 10),
                Size = new System.Drawing.Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new System.Drawing.Font("Comic Sans MS", 12F)
            };
            this.Controls.Add(cmbUsuarios);
            grid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(540, 380),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White
            };
            this.Controls.Add(grid);
            // Cargar usuarios y seleccionar el actual
            var usuarios = dbService.ObtenerUsuarios();
            cmbUsuarios.DataSource = usuarios;
            cmbUsuarios.DisplayMember = "Nombre";
            cmbUsuarios.ValueMember = "Id";
            if (usuarios.Rows.Count > 0)
            {
                cmbUsuarios.SelectedValue = usuarioId;
            }
            cmbUsuarios.SelectedIndexChanged += (s, e) => MostrarHistorial();
            MostrarHistorial();
        }
        private void MostrarHistorial()
        {
            if (cmbUsuarios.SelectedValue is int id)
            {
                var dt = dbService.ObtenerHistorial(id);
                grid.DataSource = dt;
            }
        }
    }

    // Formulario para Vocabulario
    public class VocabularyForm : Form
    {
        private Label lblWord;
        private TextBox txtWord;
        private Button btnExplain;
        private TextBox txtExplanation;
        private OpenAIService openAIService;

        public VocabularyForm()
        {
            this.Text = "Vocabulario";
            this.Size = new System.Drawing.Size(520, 500); // antes 400x350
            this.BackColor = System.Drawing.Color.FromArgb(255, 255, 240, 245); // Rosa pastel

            lblWord = new Label
            {
                Text = "Palabra:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(80, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.MediumVioletRed
            };
            txtWord = new TextBox
            {
                Location = new System.Drawing.Point(110, 20),
                Size = new System.Drawing.Size(200, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                BackColor = System.Drawing.Color.White
            };
            btnExplain = new Button
            {
                Text = "Explicar",
                Location = new System.Drawing.Point(320, 20),
                Size = new System.Drawing.Size(60, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightPink,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnExplain.FlatAppearance.BorderSize = 0;
            btnExplain.Click += BtnExplain_Click;
            txtExplanation = new TextBox
            {
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(360, 200),
                Font = new System.Drawing.Font("Comic Sans MS", 12F),
                Multiline = true,
                ReadOnly = true,
                BackColor = System.Drawing.Color.WhiteSmoke
            };
            this.Controls.Add(lblWord);
            this.Controls.Add(txtWord);
            this.Controls.Add(btnExplain);
            this.Controls.Add(txtExplanation);

            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Por favor, configura la variable de entorno OPENAI_API_KEY con tu clave de OpenAI.", "API Key requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                openAIService = null;
                this.Close();
                return;
            }
            openAIService = new OpenAIService(apiKey);
        }

#pragma warning disable CS8765
        // Asegurar que la firma del evento no use tipos anulables
        private async void BtnExplain_Click(object sender, EventArgs e)
        {
            if (openAIService == null) return;
            string word = txtWord.Text.Trim();
            if (string.IsNullOrEmpty(word)) return;
            btnExplain.Enabled = false;
            try
            {
                string prompt = $"Explica la palabra '{word}' en inglés para un niño de 7 a 12 años, usando ejemplos sencillos y una definición fácil.";
                string result = await openAIService.GetChatResponseAsync(prompt);
                txtExplanation.Text = result;
            }
            catch (Exception ex)
            {
                txtExplanation.Text = $"[Error]: {ex.Message}";
            }
            btnExplain.Enabled = true;
        }
#pragma warning restore CS8765
    }

    public class OpenAIService
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;
        private const string endpoint = "https://api.openai.com/v1/chat/completions";

        public OpenAIService(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Eres un tutor amable y didáctico que enseña inglés a niños de 7 a 12 años. Usa ejemplos sencillos, lenguaje claro y responde de forma divertida." },
                    new { role = "user", content = userMessage }
                },
                max_tokens = 200
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var result = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return result ?? string.Empty;
        }
    }

    // Formulario para Login
    public class LoginForm : Form
    {
        public string NombreUsuario;
        private TextBox txtNombre;
        private Button btnEntrar;

        public LoginForm()
        {
            this.Text = "¡Bienvenido a EnglishFun AI!";
            this.Size = new System.Drawing.Size(520, 350); // antes 400x250
            this.BackColor = System.Drawing.Color.FromArgb(255, 230, 245, 255); // Azul pastel
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            var icon = new PictureBox
            {
                Image = System.Drawing.SystemIcons.Information.ToBitmap(), // Puedes reemplazar por un ícono infantil
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(60, 60)
            };
            var lbl = new Label
            {
                Text = "¿Cómo te llamas?",
                Location = new System.Drawing.Point(110, 30),
                Size = new System.Drawing.Size(250, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.MediumVioletRed
            };
            txtNombre = new TextBox
            {
                Location = new System.Drawing.Point(110, 70),
                Size = new System.Drawing.Size(230, 35),
                Font = new System.Drawing.Font("Comic Sans MS", 14F)
            };
            btnEntrar = new Button
            {
                Text = "¡Entrar!",
                Location = new System.Drawing.Point(110, 130),
                Size = new System.Drawing.Size(120, 45),
                Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightPink,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.Click += BtnEntrar_Click;

            this.Controls.Add(icon);
            this.Controls.Add(lbl);
            this.Controls.Add(txtNombre);
            this.Controls.Add(btnEntrar);
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                NombreUsuario = txtNombre.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, escribe tu nombre.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // Juego: Completa la Palabra
    public class CompletarPalabraForm : Form
    {
        private Label lblInstruccion;
        private Label lblPalabra;
        private TextBox txtRespuesta;
        private Button btnVerificar;
        private string[] palabras = { "c_t", "d_g", "h_ouse", "c_r", "a_ple" };
        private string[] respuestas = { "cat", "dog", "house", "car", "apple" };
        private int actual = 0;
        private int puntaje = 0;

        public CompletarPalabraForm()
        {
            this.Text = "Completa la Palabra";
            this.Size = new System.Drawing.Size(520, 350); // antes 400x250
            this.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 220);

            lblInstruccion = new Label
            {
                Text = "Escribe la letra que falta:",
                Location = new System.Drawing.Point(30, 20),
                Size = new System.Drawing.Size(340, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.MediumVioletRed
            };
            lblPalabra = new Label
            {
                Location = new System.Drawing.Point(30, 60),
                Size = new System.Drawing.Size(340, 40),
                Font = new System.Drawing.Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue
            };
            txtRespuesta = new TextBox
            {
                Location = new System.Drawing.Point(30, 110),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Comic Sans MS", 14F)
            };
            btnVerificar = new Button
            {
                Text = "Verificar",
                Location = new System.Drawing.Point(250, 110),
                Size = new System.Drawing.Size(100, 35),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightSkyBlue,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnVerificar.FlatAppearance.BorderSize = 0;
            btnVerificar.Click += BtnVerificar_Click;

            this.Controls.Add(lblInstruccion);
            this.Controls.Add(lblPalabra);
            this.Controls.Add(txtRespuesta);
            this.Controls.Add(btnVerificar);
            MostrarPalabra();
        }

        private void MostrarPalabra()
        {
            if (actual >= palabras.Length)
            {
                MessageBox.Show($"¡Juego terminado! Puntaje: {puntaje}/{palabras.Length}", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            lblPalabra.Text = palabras[actual];
            txtRespuesta.Text = "";
        }

        private void BtnVerificar_Click(object sender, EventArgs e)
        {
            if (txtRespuesta.Text.Trim().ToLower() == respuestas[actual])
            {
                puntaje++;
                MessageBox.Show("¡Correcto!", "Bien hecho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Incorrecto. La respuesta era: {respuestas[actual]}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            actual++;
            MostrarPalabra();
        }
    }

    // Juego: Adivina la Imagen (versión texto)
    public class ImagenJuegoForm : Form
    {
        private Label lblInstruccion;
        private PictureBox picImagen;
        private TextBox txtRespuesta;
        private Button btnVerificar;
        private string[] imagenes = { "cat.png", "dog.png", "apple.png", "car.png", "house.png" };
        private string[] respuestas = { "cat", "dog", "apple", "car", "house" };
        private int actual = 0;
        private int puntaje = 0;

        public ImagenJuegoForm()
        {
            this.Text = "Adivina la Imagen";
            this.Size = new System.Drawing.Size(520, 500); // antes 400x350
            this.BackColor = System.Drawing.Color.FromArgb(255, 240, 255, 255);

            lblInstruccion = new Label
            {
                Text = "¿Qué ves en la imagen? (escribe en inglés)",
                Location = new System.Drawing.Point(30, 20),
                Size = new System.Drawing.Size(340, 30),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.MediumVioletRed
            };
            picImagen = new PictureBox
            {
                Location = new System.Drawing.Point(100, 60),
                Size = new System.Drawing.Size(180, 120),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtRespuesta = new TextBox
            {
                Location = new System.Drawing.Point(30, 200),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Comic Sans MS", 14F)
            };
            btnVerificar = new Button
            {
                Text = "Verificar",
                Location = new System.Drawing.Point(250, 200),
                Size = new System.Drawing.Size(100, 35),
                Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightSkyBlue,
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnVerificar.FlatAppearance.BorderSize = 0;
            btnVerificar.Click += BtnVerificar_Click;

            this.Controls.Add(lblInstruccion);
            this.Controls.Add(picImagen);
            this.Controls.Add(txtRespuesta);
            this.Controls.Add(btnVerificar);
            MostrarImagen();
        }

        private void MostrarImagen()
        {
            if (actual >= imagenes.Length)
            {
                MessageBox.Show($"¡Juego terminado! Puntaje: {puntaje}/{imagenes.Length}", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            string ruta = System.IO.Path.Combine("C:\\Users\\Mario\\Desktop\\English Fun AI\\imagenes", imagenes[actual]);
            if (System.IO.File.Exists(ruta))
                picImagen.Image = Image.FromFile(ruta);
            else
                picImagen.Image = null;
            txtRespuesta.Text = "";
        }
        private void BtnVerificar_Click(object sender, EventArgs e)
        {
            if (txtRespuesta.Text.Trim().ToLower() == respuestas[actual])
            {
                puntaje++;
                MessageBox.Show("¡Correcto!", "Bien hecho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Incorrecto. La respuesta era: {respuestas[actual]}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            actual++;
            MostrarImagen();
        }
    }

    // Clase para conexión y operaciones básicas
    public class DatabaseService
    {
        private readonly string connectionString;
        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
            EnsureDatabase();
        }
        public void EnsureDatabase()
        {
            using var connection = new SqlConnection(connectionString.Replace("Database=EnglishFunAI;", ""));
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "IF DB_ID('EnglishFunAI') IS NULL CREATE DATABASE EnglishFunAI";
            try { cmd.ExecuteNonQuery(); } catch { }
            cmd.CommandText = "USE EnglishFunAI";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' and xtype='U')
                CREATE TABLE Usuarios (
                    Id INT PRIMARY KEY IDENTITY,
                    Nombre NVARCHAR(50) NOT NULL,
                    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
                )";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Progreso' and xtype='U')
                CREATE TABLE Progreso (
                    Id INT PRIMARY KEY IDENTITY,
                    UsuarioId INT NOT NULL,
                    Modulo NVARCHAR(30) NOT NULL,
                    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
                    Detalle NVARCHAR(200),
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                )";
            cmd.ExecuteNonQuery();
        }
        public int RegistrarUsuario(string nombre)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id FROM Usuarios WHERE Nombre = @nombre";
            cmd.Parameters.AddWithValue("@nombre", nombre);
            var result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
                return (int)result;
            cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Usuarios (Nombre) OUTPUT INSERTED.Id VALUES (@nombre)";
            cmd.Parameters.AddWithValue("@nombre", nombre);
            return (int)cmd.ExecuteScalar();
        }
        public void RegistrarProgreso(int usuarioId, string modulo, string detalle)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Progreso (UsuarioId, Modulo, Detalle) VALUES (@usuarioId, @modulo, @detalle)";
            cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@modulo", modulo);
            cmd.Parameters.AddWithValue("@detalle", detalle);
            cmd.ExecuteNonQuery();
        }
        public DataTable ObtenerHistorial(int usuarioId)
        {
            var dt = new DataTable();
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Modulo, Fecha, Detalle FROM Progreso WHERE UsuarioId = @usuarioId ORDER BY Fecha DESC";
            cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }
        // En DatabaseService, agregar método para obtener todos los usuarios:
        public DataTable ObtenerUsuarios()
        {
            var dt = new DataTable();
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre FROM Usuarios ORDER BY Nombre";
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }
    }
}
