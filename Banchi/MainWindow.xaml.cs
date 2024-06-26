﻿using Banchi.Classi;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Label = System.Windows.Controls.Label;

namespace Banchi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Aula aulaCorrente;
        private Banco bancoCorrente;
        private Classe classeCorrente;

        //private Studente studenteCorrente;

        internal Label labelSelezionata;

        List<Aula> listaAuleUtente;
        List<Classe> listaClassiUtente;

        List<Aula> listaAuleModello;
        List<Classe> listaClassiModello;
        List<Computer> listaComputer;
        List<Studente> listaDistribuzioneBanco;

        public MainWindow()
        {
            InitializeComponent();
            BusinessLayer.Inizializzazioni();

            if (Utente.Accesso != Utente.RuoloUtente.ModificheAiModelli)
            {
                btn_Salva.Visibility = Visibility.Hidden;
            }
            // legge tutte le aule dal "database" e le mette in una lista
            listaAuleUtente = BusinessLayer.LeggiTutteLeAuleUtente();
            // riempimento del ComboBox con le aule appena lette
            if (listaAuleUtente != null)
                foreach (Aula a in listaAuleUtente)
                {
                    // un combo può contenere oggetti di qualsiasi tipo, però per vederci qualcosa dentro
                    // devo implementare il metodo ToString() dentro listaAuleUtente classe Aule
                    cmbAuleUtente.Items.Add(a);
                }

            listaClassiUtente = BusinessLayer.LeggiTutteLeClassiUtente();
            if (listaAuleUtente != null)
                foreach (Classe a in listaClassiUtente)
                {
                    cmbClasseUtente.Items.Add(a);
                }

            // riempio i combobox dei modelli
            listaAuleModello = BusinessLayer.LeggiTutteLeAule();
            // riempimento del ComboBox con le aule appena lette
            foreach (Aula a in listaAuleModello)
            {
                // un combo può contenere oggetti di qualsiasi tipo, però per vederci qualcosa dentro
                // devo implementare il metodo ToString() dentro listaAuleUtente classe Aule
                cmbModelliAule.Items.Add(a);
            }
            // esempi di inizializzazione di un ComboBox
            //cmbModelliAule.SelectedIndex = 1; // seleziona listaAuleUtente seconda aula
            //cmbModelliAule.SelectedItem = cmbAuleUtente.Items[1]; // seleziona listaAuleUtente seconda aula
            // recupera il nome dell'aula selezionata 
            //string aulaSelezionata = ((Aula)cmbAule.SelectedItem).NomeAula;
            // recupera l'altezza dell'aula selezionata
            //double altezzaAula = ((Aula)cmbAule.SelectedItem).AltezzaInCentimetri;
            listaClassiModello = BusinessLayer.LeggiTutteLeClassi();
            // riempimento del ComboBox con le aule appena lette
            foreach (Classe c in listaClassiModello)
            {
                // un combo può contenere oggetti di qualsiasi tipo, però per vederci qualcosa dentro
                // devo implementare il metodo ToString() dentro listaAuleUtente classe Aule
                cmbModelliClasse.Items.Add(c);
            }
            //cmbModelliClasse.SelectedIndex = 1;
            //cmbModelliClasse.SelectedItem = cmbClasseUtente.Items[1];

            listaComputer = BusinessLayer.LeggiTuttiIComputer();
            // riempimento del ComboBox con i computer appena letti
            foreach (Computer c in listaComputer)
            {
                lstComputer.Items.Add(c);
            }
            Panel.SetZIndex(lstComputer, 10000);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // evento che viene lanciato alla fine del caricamento della finestra 
            // metodo di prova che crea un'intera aula con pochi banchi 
            //aula = CreaAulaDiProva();
            //aula.MettiInScalaAulaEBanchi();
            // cartiglio
            Label graficaCartiglio = new();
            AreaDisegno.Children.Add(graficaCartiglio);
            Cartiglio c = new Cartiglio(graficaCartiglio, aulaCorrente, classeCorrente, Utente.Username);
        }
        private void MenuAula_Click(object sender, RoutedEventArgs e)
        {
            ApriFinestraAula();
        }
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow wnd = new AboutWindow();
            wnd.Show();
        }
        private void btn_Banchi_Click(object sender, RoutedEventArgs e)
        {
            ApriFinestraBanchi();
        }
        private void ApriFinestraBanchi()
        {
            if (cmbModelliAule.SelectedItem == null)
            {
                MessageBox.Show("Selezionare un'aula fra i dati condivisi", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BanchiWindow wnd = new BanchiWindow((Aula)cmbModelliAule.SelectedItem);
            wnd.Show();
        }
        private void btn_Aula_Click(object sender, RoutedEventArgs e)
        {
            ApriFinestraAula();
        }
        private void ApriFinestraAula()
        {
            //if (cmbModelliAule.SelectedItem == null)
            //{
            //    MessageBox.Show("Selezionare un'aula fra i dati condivisi", "Errore",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            AulaWindow wnd = new AulaWindow(aulaCorrente);
            wnd.Show();
        }
        private void btn_SalvataggioCondivisi_Click(object sender, RoutedEventArgs e)
        {
            BusinessLayer.ScriviTutteLeAule(listaAuleModello);
        }
        private void cmbModelliClasse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                if (cmbModelliClasse.SelectedItem != null)
                {
                    List<Studente> listaStudenti = BusinessLayer.LeggiStudentiClasse((Classe)cmbModelliClasse.SelectedItem);
                    lstStudenti.ItemsSource = listaStudenti;
                    listaDistribuzioneBanco = listaStudenti;
                }
                else
                {
                    lstStudenti.ItemsSource = null;
                }
                chkStudenti.IsChecked = true;
            }
        }
        private void cmbModelliAule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // cancella tutti i controlli dell'aula precedente
            AreaDisegno.Children.Clear();
            if (cmbModelliAule.SelectedItem != null)
            {
                aulaCorrente = ((Aula)cmbModelliAule.SelectedItem);
                // aula farlocca, in attesa di completare la lettura dei dati 
                //aulaCorrente = CreaAulaDiProva();
                CreaAulaGrafica(aulaCorrente);
                //aulaCorrente.MettiInScalaAulaEBanchi();
            }
        }
        private void CreaAulaGrafica(Aula aula)
        {
            Label GraficaAula = new Label();
            AreaDisegno.Children.Add(GraficaAula);
            if (aula == null)
                aula = new Aula("prova", 8000, 12000, GraficaAula);
            else
                aula.GraficaAula = GraficaAula;
            // creazione di tutti i nuovi banchi grafici
            int zIndexBanco = 100;
            foreach (Banco b in aula.Banchi)
            {
                Label GraficaBanco = new();
                // metodo delegato per gestione click
                GraficaBanco.MouseDown += ClickSuBanco;
                AreaDisegno.Children.Add(GraficaBanco);
                b.GraficaBanco = GraficaBanco;
                //Banco bancoNuovo = new Banco(false, b.BaseInCentimetri,
                //    b.AltezzaInCentimetri, b.PosizioneXInPixel, b.PosizioneYInPixel, GraficaBanco);
                //// aggiunta del banco appena fatto all'aula
                //aula.Banchi.Add(bancoNuovo);
                Panel.SetZIndex(GraficaBanco, zIndexBanco);
                zIndexBanco++;
            }
        }
        private void chkStudenti_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                lstStudenti.Visibility = Visibility.Visible;
        }
        private void chkStudenti_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                lstStudenti.Visibility = Visibility.Collapsed;
        }
        private void chkComputer_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                lstComputer.Visibility = Visibility.Visible;
        }
        private void chkComputer_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                lstComputer.Visibility = Visibility.Collapsed;
        }
        private void btn_Computer_Click(object sender, RoutedEventArgs e)
        {
            ApriFinestraComputer();
        }
        private void ApriFinestraComputer()
        {
            //if (cmbModelliAule.SelectedItem == null)
            //{
            //    MessageBox.Show("Selezionare un'aula fra i dati condivisi", "Errore",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            Computer computer;
            computer = (Computer)lstComputer.SelectedItem;



            MessageBoxButton bottone = MessageBoxButton.YesNo;
            MessageBoxResult result;
            ComputerWindow wnd = new ComputerWindow((Aula)cmbModelliAule.SelectedItem, (Computer)lstComputer.SelectedItem);
            string messageboxtext = "Non hai selezionato un computer, vuoi crearne uno nuovo?";
            string messageboxcaption = "errore";

            MessageBoxImage messageBoxImage = MessageBoxImage.Question;


            if (computer == null)
            {
                result = MessageBox.Show(messageboxtext, messageboxcaption, bottone, messageBoxImage, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    wnd.Show();
                }
            }

            if (computer != null)
            {
                wnd.Show();
            }
        }
        private void btn_Classe_Click(object sender, RoutedEventArgs e)
        {
            ApriFinestraClasse();
        }
        private void ApriFinestraClasse()
        {
            //if (cmbModelliAule.SelectedItem == null)
            //{
            //    MessageBox.Show("Selezionare un'aula fra i dati condivisi", "Errore",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            // se non c'è nulla di selezionato, la finestra aperta dovrà creare una nuova classe 
            ClasseWindow wnd = new ClasseWindow((Classe)cmbModelliClasse.SelectedItem);
            wnd.Show();
        }
        private void btn_AssociaStudente_Click(object sender, RoutedEventArgs e)
        {
            if (lstStudenti.SelectedItem != null && bancoCorrente != null)
            {
                bancoCorrente.Studente = (Studente)lstStudenti.SelectedItem;
                bancoCorrente.AggiungiTestoAGrafica();
                bancoCorrente.GraficaBanco.BorderBrush = Brushes.LightCoral;
            }
        }
        private void btn_AssociaComputer_Click(object sender, RoutedEventArgs e)
        {
            if (lstComputer.SelectedItem != null && bancoCorrente != null)
            {
                bancoCorrente.Computer = (Computer)lstComputer.SelectedItem;
                bancoCorrente.AggiungiTestoAGrafica();
                bancoCorrente.GraficaBanco.BorderBrush = Brushes.LightCoral;
            }
        }
        private void ClickSuBanco(object sender, RoutedEventArgs e)
        {
            labelSelezionata = (Label)sender;
            // cerca nei banchi dell'aula quello che è stato cliccato
            foreach (Banco b in aulaCorrente.Banchi)
            {
                if (b.GraficaBanco == labelSelezionata)
                {
                    bancoCorrente = b;
                    labelSelezionata.BorderBrush = Brushes.Red;
                }
                else
                {
                    b.GraficaBanco.BorderBrush = Brushes.Black;
                }
            }
        }
        private void cmbBanchiEStudenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO visualizzare l'aula con gli studenti 
        }
        private void btn_SalvataggioUtente_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_DistribuisciStudenti_Click(object sender, RoutedEventArgs e)
        {
            if (listaDistribuzioneBanco != null && aulaCorrente != null)
            {
                ////questi commenti servono a vedere se l'ordinamento voti funziona
                //listaDistribuzioneBanco[0].Voto = 0.0;
                //listaDistribuzioneBanco[1].Voto = 1.0;
                //listaDistribuzioneBanco[2].Voto = 2.0;
                //listaDistribuzioneBanco[3].Voto = 3.0;
                //listaDistribuzioneBanco[4].Voto = 4.0;
                //listaDistribuzioneBanco[5].Voto = 5.0;
                //listaDistribuzioneBanco[6].Voto = 6.0;
                //listaDistribuzioneBanco[7].Voto = 7.0;
                //listaDistribuzioneBanco[8].Voto = 8.0;
                //listaDistribuzioneBanco[9].Voto = 9.0;
                //listaDistribuzioneBanco[10].Voto = 10.0;
                //listaDistribuzioneBanco[11].Voto = 11.0;
                //listaDistribuzioneBanco[12].Voto = 12.0;
                //listaDistribuzioneBanco[13].Voto = 13.0;
                //listaDistribuzioneBanco[14].Voto = 14.0;
                //listaDistribuzioneBanco[15].Voto = 15.0;
                //listaDistribuzioneBanco[16].Voto = 16.0;

                if (rdbCasuale.IsChecked == true)
                    listaDistribuzioneBanco = BusinessLayer.ordinamentoCasualeListaStudenti(listaDistribuzioneBanco);
                else
                {
                    if (rdbAlfabetico.IsChecked == true)
                        listaDistribuzioneBanco = BusinessLayer.ordinamentoAlfabeticoListaStudenti(listaDistribuzioneBanco);
                    else
                        listaDistribuzioneBanco = BusinessLayer.ordinamentoVotoListaStudenti(listaDistribuzioneBanco);
                }


                int minimoLunghezzaListe = aulaCorrente.Banchi.Count();
                if (listaDistribuzioneBanco.Count() < minimoLunghezzaListe)
                    minimoLunghezzaListe = listaDistribuzioneBanco.Count();
                for (int i = 0; i < minimoLunghezzaListe; i++)
                {
                    aulaCorrente.Banchi[i].Studente = listaDistribuzioneBanco[i];
                    aulaCorrente.Banchi[i].AggiungiTestoAGrafica();
                }
            }
        }
        private void txtFiltroComputer_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filtraggio dei computer 
            string filterText = txtFiltroComputer.Text.ToLower(); // Converti in minuscolo per confronto case-insensitive
            lstComputer.Items.Clear(); // Rimuovi tutti gli elementi dalla ListBox
            // metti nella ListBox solo gli elementi che corrispondono alla stringa di filtro 
            foreach (Computer item in listaComputer)
            {
                if (item.ToString().ToLower().Contains(filterText))
                {
                    lstComputer.Items.Add(item); // Aggiungi solo gli elementi che corrispondono al filtro
                }
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.IsLoaded && aulaCorrente != null)
                aulaCorrente.MettiInScalaAulaEBanchi();
        }
        private Aula CreaAulaDiProva()
        {
            // QUESTO METODO CREA UN'AULA DI PROVA E DOVRA' ESSERE CANCELLATO QUANDO LE 
            // AULE CARICATE NEL COMBOBOX AVRANNO AL LORO INTERNO LE INFORMAZIONI
            // CHE METTIAMO NEL SEGUENTE CODICE 
            // creazione aula
            Label GraficaAula = new Label();
            AreaDisegno.Children.Add(GraficaAula);

            Aula aula = new Aula("prova", 8000, 12000, GraficaAula);
            // creazione di un nuovo banco
            Label GraficaBanco = new();
            // metodo delegato per gestione click
            GraficaBanco.MouseDown += ClickSuBanco;
            AreaDisegno.Children.Add(GraficaBanco);
            Banco bancoNuovo = new Banco(false, 100, 80, 250, 100, GraficaBanco);
            // aggiunta del banco appena fatto all'aula
            aula.Banchi.Add(bancoNuovo);
            Panel.SetZIndex(GraficaBanco, 100);

            GraficaBanco = new();
            // metodo delegato per gestione click
            GraficaBanco.MouseDown += ClickSuBanco;
            AreaDisegno.Children.Add(GraficaBanco);
            bancoNuovo = new Banco(false, 100, 80, 250, 200, GraficaBanco);
            // aggiunta del banco appena fatto all'aula
            aula.Banchi.Add(bancoNuovo);
            Panel.SetZIndex(GraficaBanco, 101);

            return aula;
        }
        private void chkCartiglio_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void chkCartiglio_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}