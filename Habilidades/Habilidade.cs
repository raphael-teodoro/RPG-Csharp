using projeto1_RPG.Personagens.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeto1_RPG.Habilidades
{
	internal abstract class Habilidade
	{
		public string Nome { get; set; }
		public int Custo { get; set; }
		public bool UsarAliado { get; set; }

		public Habilidade(string nome, int custo)
		{
			Nome = nome;
			Custo = custo;
			UsarAliado = true;
		}

		public bool PodeUsar(Personagem personagem)
		{
			string msg;
			return PodeUsar(personagem, out msg);
		}

		public virtual bool PodeUsar(Personagem personagem, out string msg)
		{
			msg = String.Empty;
			if (personagem.PtsHabiliAtual < Custo) msg=$"Você não possui {personagem.Classe.GetDescPtsHabili()} suficiente para usar esta habilidade.";
			return (msg==String.Empty);
		}

		public virtual void Usar(Personagem origem, Personagem alvo)
		{
            origem.PtsHabiliAtual -= Custo;
        }
	}
}