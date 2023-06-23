using projeto1_RPG.Personagens.Racas;
using projeto1_RPG.Personagens.Classes;
using projeto1_RPG.Personagens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeto1_RPG.Combate
{
    internal class GeradorOponentes
    {
        public enum Dificuldade
        {
            Nenhum,
            Facil,
            Medio,
            Dificil,
            Impossivel
        }

        public enum Estilo
        {
            Nenhum,
            Normal,
            Grupo,
            Chefe
        }

        private T RandomEnum<T>() where T : Enum
        {
            // Ignora primeiro enum pois considera ser o "Nenhum"
            Array array = Enum.GetValues(typeof(T));
            return (T)array.GetValue(new Random().Next(1, array.Length));
        }

        private int NivelPorDificuldade(int nivelMin, int nivelMax, Dificuldade dificuldade, int qtdJogadores)
        {
            switch (dificuldade)
            {
                case Dificuldade.Facil:
                    nivelMin = Math.Max(1, nivelMin - 1);
                    nivelMax = Math.Max(1, nivelMax - (nivelMax-nivelMin) / 2);
                    break;
                case Dificuldade.Dificil:
                    nivelMin += (nivelMax-nivelMin) / 2;
                    break;
                case Dificuldade.Impossivel:
                    nivelMin += (nivelMax-nivelMin) / 2;
                    nivelMax += qtdJogadores;
                    break;
            }
            return new Random().Next(nivelMin,nivelMax+1);
        }

        public List<Personagem> MontarLista(List<Personagem> jogadores, Dificuldade dificuldade = Dificuldade.Nenhum, Estilo estilo = Estilo.Nenhum)
        {
            List<Personagem> oponentes = new List<Personagem>();
            if (dificuldade == Dificuldade.Nenhum) dificuldade = RandomEnum<Dificuldade>();
            if (estilo == Estilo.Nenhum) estilo = RandomEnum<Estilo>();

            int nivelMin;
            int nivelMax;
            int nivel;
            switch (estilo)
            {
                case Estilo.Chefe:
                    // Um oponente forte
                    nivelMin = jogadores.Max(j => j.Nivel.NivelAtual);
                    nivelMax = nivelMin + (int)Math.Pow(jogadores.Count,2) + 1;
                    nivel = NivelPorDificuldade(nivelMin, nivelMax, dificuldade, jogadores.Count);

                    oponentes.Add(new Oponente(ListaRacas.RandomOponente(), ListaClasses.Random(), nivel));
                    break;

                case Estilo.Grupo:
                    // Vários oponentes fracos
                    Raca raca = ListaRacas.RandomOponente();
                    double somaNiveis = jogadores.Sum(j => j.Nivel.NivelAtual);
                    nivelMax = (int)Math.Floor(somaNiveis / jogadores.Count * 0.65);
                    nivelMin = Math.Max(1,nivelMax-3);
                    somaNiveis *= 1.2;

                    while (somaNiveis > 0)
                    {
                        nivel = NivelPorDificuldade(nivelMin, nivelMax, dificuldade, jogadores.Count);
                        somaNiveis -= nivel;
                        oponentes.Add(new Oponente(raca, ListaClasses.Random(), nivel));
                    }
                    break;

                default:
                    // Um oponente médio por jogador
                    foreach (Personagem p in jogadores)
                    {
                        nivelMin = p.Nivel.NivelAtual-2;
                        nivelMax = p.Nivel.NivelAtual+1;
                        nivel = NivelPorDificuldade(nivelMin, nivelMax, dificuldade, jogadores.Count);

                        oponentes.Add(new Oponente(ListaRacas.RandomOponente(), ListaClasses.Random(), nivel));
                    }
                    break;
            }
            return oponentes;
        }
    }
}