using System;
using System.IO;
using System.Text;

namespace Update.Classes.Functions;

internal class CabalConfigEditor
{
	public class SettingsHandler
	{
		private uint[] unk;

		private byte unkb;

		private uint[] unkEX;

		private string account;

		public string Directory;

		public uint Width;

		public uint Height;

		public uint Width_EX;

		public uint Height_EX;

		public uint RefreshRate;

		public float Gamma;

		public uint SoundVolume;

		public uint AmbienceVolume;

		public bool ShowWeatherEffects;

		public bool ShowDistortionEffects;

		public uint ShadowLevel;

		public uint WaterQuality;

		public uint BlurLevel;

		public Ratio ScreenRatio;

		public bool AllowWhispers;

		public bool AllowPartyInvites;

		public bool AllowTrades;

		public bool AllowPvPRequest;

		public bool ShowPlayerName;

		public bool ShowMonsterName;

		public bool ShowNPCName;

		public bool ShowItemName;

		public bool ShowGuildName;

		public bool AutoAttack;

		public View CameraView;

		public bool ShowCharacterHP;

		public bool ShowMonsterHP;

		public bool UseFunctionKeys;

		public bool ShowShouts;

		public bool ShowDialogueBubbles;

		public bool ShowComboUIonTop;

		public bool DashtoCursor;

		public DisplayModeName DisplayMode;

		public uint JukeboxVolume;

		public uint RenderDistance;

		public uint SpecialEffectLevel;

		public Language LanguageCode;

		public SettingsHandler()
		{
			Directory = "";
			unk = new uint[11];
			unkEX = new uint[62];
		}

		public SettingsHandler(string directory)
			: this()
		{
			Directory = directory;
		}

		public void Open()
		{
			BinaryReader binaryReader = new BinaryReader(File.OpenRead(Directory + "main.dat"));
			Width = binaryReader.ReadUInt32();
			Height = binaryReader.ReadUInt32();
			RefreshRate = binaryReader.ReadUInt32();
			unk[0] = binaryReader.ReadUInt32();
			Gamma = binaryReader.ReadSingle();
			unk[1] = binaryReader.ReadUInt32();
			unk[2] = binaryReader.ReadUInt32();
			SoundVolume = binaryReader.ReadUInt32();
			AmbienceVolume = binaryReader.ReadUInt32();
			ShowWeatherEffects = binaryReader.ReadUInt32() == 1;
			unk[3] = binaryReader.ReadUInt32();
			ShadowLevel = binaryReader.ReadUInt32();
			WaterQuality = binaryReader.ReadUInt32();
			BlurLevel = binaryReader.ReadUInt32();
			ScreenRatio = (Ratio)binaryReader.ReadUInt32();
			AllowWhispers = binaryReader.ReadUInt32() == 1;
			AllowPartyInvites = binaryReader.ReadUInt32() == 1;
			AllowTrades = binaryReader.ReadUInt32() == 1;
			AllowPvPRequest = binaryReader.ReadUInt32() == 1;
			ShowPlayerName = binaryReader.ReadUInt32() == 1;
			unk[4] = binaryReader.ReadUInt32();
			ShowMonsterName = binaryReader.ReadUInt32() == 1;
			unk[5] = binaryReader.ReadUInt32();
			ShowNPCName = binaryReader.ReadUInt32() == 1;
			unk[6] = binaryReader.ReadUInt32();
			ShowItemName = binaryReader.ReadUInt32() == 1;
			unk[7] = binaryReader.ReadUInt32();
			ShowDistortionEffects = binaryReader.ReadUInt32() == 1;
			AutoAttack = binaryReader.ReadUInt32() == 1;
			CameraView = (View)binaryReader.ReadUInt32();
			account = Encoding.ASCII.GetString(binaryReader.ReadBytes(15));
			ShowCharacterHP = binaryReader.ReadUInt32() == 1;
			ShowMonsterHP = binaryReader.ReadUInt32() == 1;
			UseFunctionKeys = binaryReader.ReadUInt32() == 1;
			ShowGuildName = binaryReader.ReadUInt32() == 1;
			ShowShouts = binaryReader.ReadUInt32() == 1;
			ShowDialogueBubbles = binaryReader.ReadUInt32() == 1;
			unk[8] = binaryReader.ReadUInt32();
			unk[9] = binaryReader.ReadUInt32();
			unk[10] = binaryReader.ReadUInt32();
			binaryReader.Close();
			binaryReader = new BinaryReader(File.OpenRead(Directory + "mainEX.dat"));
			unkb = binaryReader.ReadByte();
			ShowComboUIonTop = binaryReader.ReadUInt32() == 1;
			DashtoCursor = binaryReader.ReadUInt32() == 1;
			DisplayMode = (DisplayModeName)binaryReader.ReadUInt32();
			Width_EX = binaryReader.ReadUInt32();
			Height_EX = binaryReader.ReadUInt32();
			JukeboxVolume = binaryReader.ReadUInt32();
			unkEX[0] = binaryReader.ReadUInt32();
			RenderDistance = binaryReader.ReadUInt32();
			SpecialEffectLevel = binaryReader.ReadUInt32();
			unkEX[1] = binaryReader.ReadUInt32();
			unkEX[2] = binaryReader.ReadUInt32();
			unkEX[3] = binaryReader.ReadUInt32();
			unkEX[4] = binaryReader.ReadUInt32();
			unkEX[5] = binaryReader.ReadUInt32();
			unkEX[6] = binaryReader.ReadUInt32();
			unkEX[7] = binaryReader.ReadUInt32();
			unkEX[8] = binaryReader.ReadUInt32();
			unkEX[9] = binaryReader.ReadUInt32();
			unkEX[10] = binaryReader.ReadUInt32();
			unkEX[11] = binaryReader.ReadUInt32();
			unkEX[12] = binaryReader.ReadUInt32();
			unkEX[13] = binaryReader.ReadUInt32();
			unkEX[14] = binaryReader.ReadUInt32();
			unkEX[15] = binaryReader.ReadUInt32();
			unkEX[16] = binaryReader.ReadUInt32();
			unkEX[17] = binaryReader.ReadUInt32();
			unkEX[18] = binaryReader.ReadUInt32();
			unkEX[19] = binaryReader.ReadUInt32();
			unkEX[20] = binaryReader.ReadUInt32();
			LanguageCode = (Language)binaryReader.ReadUInt32();
			unkEX[21] = binaryReader.ReadUInt32();
			unkEX[22] = binaryReader.ReadUInt32();
			unkEX[23] = binaryReader.ReadUInt32();
			unkEX[24] = binaryReader.ReadUInt32();
			unkEX[25] = binaryReader.ReadUInt32();
			unkEX[26] = binaryReader.ReadUInt32();
			unkEX[27] = binaryReader.ReadUInt32();
			unkEX[28] = binaryReader.ReadUInt32();
			unkEX[29] = binaryReader.ReadUInt32();
			unkEX[30] = binaryReader.ReadUInt32();
			unkEX[31] = binaryReader.ReadUInt32();
			unkEX[32] = binaryReader.ReadUInt32();
			unkEX[33] = binaryReader.ReadUInt32();
			unkEX[34] = binaryReader.ReadUInt32();
			unkEX[35] = binaryReader.ReadUInt32();
			unkEX[36] = binaryReader.ReadUInt32();
			unkEX[37] = binaryReader.ReadUInt32();
			unkEX[38] = binaryReader.ReadUInt32();
			unkEX[39] = binaryReader.ReadUInt32();
			unkEX[40] = binaryReader.ReadUInt32();
			unkEX[41] = binaryReader.ReadUInt32();
			unkEX[42] = binaryReader.ReadUInt32();
			unkEX[43] = binaryReader.ReadUInt32();
			unkEX[44] = binaryReader.ReadUInt32();
			unkEX[45] = binaryReader.ReadUInt32();
			unkEX[46] = binaryReader.ReadUInt32();
			unkEX[47] = binaryReader.ReadUInt32();
			unkEX[48] = binaryReader.ReadUInt32();
			unkEX[49] = binaryReader.ReadUInt32();
			unkEX[50] = binaryReader.ReadUInt32();
			unkEX[51] = binaryReader.ReadUInt32();
			unkEX[52] = binaryReader.ReadUInt32();
			unkEX[53] = binaryReader.ReadUInt32();
			unkEX[54] = binaryReader.ReadUInt32();
			unkEX[55] = binaryReader.ReadUInt32();
			unkEX[56] = binaryReader.ReadUInt32();
			unkEX[57] = binaryReader.ReadUInt32();
			unkEX[58] = binaryReader.ReadUInt32();
			unkEX[59] = binaryReader.ReadUInt32();
			unkEX[60] = binaryReader.ReadUInt32();
			unkEX[61] = binaryReader.ReadUInt32();
			binaryReader.Close();
		}

		public void Save()
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Create(Directory + "main.dat"));
			binaryWriter.Write(Width);
			binaryWriter.Write(Height);
			binaryWriter.Write(RefreshRate);
			binaryWriter.Write(unk[0]);
			binaryWriter.Write(Gamma);
			binaryWriter.Write(unk[1]);
			binaryWriter.Write(unk[2]);
			binaryWriter.Write(SoundVolume);
			binaryWriter.Write(AmbienceVolume);
			binaryWriter.Write(ShowWeatherEffects ? 1 : 0);
			binaryWriter.Write(unk[3]);
			binaryWriter.Write(ShadowLevel);
			binaryWriter.Write(WaterQuality);
			binaryWriter.Write(BlurLevel);
			binaryWriter.Write((uint)ScreenRatio);
			binaryWriter.Write(AllowWhispers ? 1 : 0);
			binaryWriter.Write(AllowPartyInvites ? 1 : 0);
			binaryWriter.Write(AllowTrades ? 1 : 0);
			binaryWriter.Write(AllowPvPRequest ? 1 : 0);
			binaryWriter.Write(ShowPlayerName ? 1 : 0);
			binaryWriter.Write(unk[4]);
			binaryWriter.Write(ShowMonsterName ? 1 : 0);
			binaryWriter.Write(unk[5]);
			binaryWriter.Write(ShowNPCName ? 1 : 0);
			binaryWriter.Write(unk[6]);
			binaryWriter.Write(ShowItemName ? 1 : 0);
			binaryWriter.Write(unk[7]);
			binaryWriter.Write(ShowDistortionEffects ? 1 : 0);
			binaryWriter.Write(AutoAttack ? 1 : 0);
			binaryWriter.Write((uint)CameraView);
			byte[] array = Encoding.ASCII.GetBytes(account);
			if (array.Length != 15)
			{
				Array.Resize(ref array, 15);
			}
			binaryWriter.Write(array);
			binaryWriter.Write(ShowCharacterHP ? 1 : 0);
			binaryWriter.Write(ShowMonsterHP ? 1 : 0);
			binaryWriter.Write(UseFunctionKeys ? 1 : 0);
			binaryWriter.Write(ShowGuildName ? 1 : 0);
			binaryWriter.Write(ShowShouts ? 1 : 0);
			binaryWriter.Write(ShowDialogueBubbles ? 1 : 0);
			binaryWriter.Write(unk[8]);
			binaryWriter.Write(unk[9]);
			binaryWriter.Write(unk[10]);
			binaryWriter.Flush();
			binaryWriter.Close();
			BinaryWriter binaryWriter2 = new BinaryWriter(File.Create(Directory + "mainEX.dat"));
			binaryWriter2.Write(unkb);
			binaryWriter2.Write(ShowComboUIonTop ? 1 : 0);
			binaryWriter2.Write(DashtoCursor ? 1 : 0);
			binaryWriter2.Write((uint)DisplayMode);
			binaryWriter2.Write(Width_EX);
			binaryWriter2.Write(Height_EX);
			binaryWriter2.Write(JukeboxVolume);
			binaryWriter2.Write(unkEX[0]);
			binaryWriter2.Write(RenderDistance);
			binaryWriter2.Write(SpecialEffectLevel);
			binaryWriter2.Write(unkEX[1]);
			binaryWriter2.Write(unkEX[2]);
			binaryWriter2.Write(unkEX[3]);
			binaryWriter2.Write(unkEX[4]);
			binaryWriter2.Write(unkEX[5]);
			binaryWriter2.Write(unkEX[6]);
			binaryWriter2.Write(unkEX[7]);
			binaryWriter2.Write(unkEX[8]);
			binaryWriter2.Write(unkEX[9]);
			binaryWriter2.Write(unkEX[10]);
			binaryWriter2.Write(unkEX[11]);
			binaryWriter2.Write(unkEX[12]);
			binaryWriter2.Write(unkEX[13]);
			binaryWriter2.Write(unkEX[14]);
			binaryWriter2.Write(unkEX[15]);
			binaryWriter2.Write(unkEX[16]);
			binaryWriter2.Write(unkEX[17]);
			binaryWriter2.Write(unkEX[18]);
			binaryWriter2.Write(unkEX[19]);
			binaryWriter2.Write(unkEX[20]);
			binaryWriter2.Write((uint)LanguageCode);
			binaryWriter2.Write(unkEX[21]);
			binaryWriter2.Write(unkEX[22]);
			binaryWriter2.Write(unkEX[23]);
			binaryWriter2.Write(unkEX[24]);
			binaryWriter2.Write(unkEX[25]);
			binaryWriter2.Write(unkEX[26]);
			binaryWriter2.Write(unkEX[27]);
			binaryWriter2.Write(unkEX[28]);
			binaryWriter2.Write(unkEX[29]);
			binaryWriter2.Write(unkEX[30]);
			binaryWriter2.Write(unkEX[31]);
			binaryWriter2.Write(unkEX[32]);
			binaryWriter2.Write(unkEX[33]);
			binaryWriter2.Write(unkEX[34]);
			binaryWriter2.Write(unkEX[35]);
			binaryWriter2.Write(unkEX[36]);
			binaryWriter2.Write(unkEX[37]);
			binaryWriter2.Write(unkEX[38]);
			binaryWriter2.Write(unkEX[39]);
			binaryWriter2.Write(unkEX[40]);
			binaryWriter2.Write(unkEX[41]);
			binaryWriter2.Write(unkEX[42]);
			binaryWriter2.Write(unkEX[43]);
			binaryWriter2.Write(unkEX[44]);
			binaryWriter2.Write(unkEX[45]);
			binaryWriter2.Write(unkEX[46]);
			binaryWriter2.Write(unkEX[47]);
			binaryWriter2.Write(unkEX[48]);
			binaryWriter2.Write(unkEX[49]);
			binaryWriter2.Write(unkEX[50]);
			binaryWriter2.Write(unkEX[51]);
			binaryWriter2.Write(unkEX[52]);
			binaryWriter2.Write(unkEX[53]);
			binaryWriter2.Write(unkEX[54]);
			binaryWriter2.Write(unkEX[55]);
			binaryWriter2.Write(unkEX[56]);
			binaryWriter2.Write(unkEX[57]);
			binaryWriter2.Write(unkEX[58]);
			binaryWriter2.Write(unkEX[59]);
			binaryWriter2.Write(unkEX[60]);
			binaryWriter2.Write(unkEX[61]);
			binaryWriter2.Flush();
			binaryWriter2.Close();
		}
	}

	public enum Ratio : uint
	{
		Recommended,
		_4_3,
		_16_9
	}

	public enum View : uint
	{
		Chase = 1u,
		Free = 2u,
		Quarter = 5u
	}

	public enum Language : uint
	{
		Korean = 1u,
		English,
		Thai,
		Japanese,
		German,
		Portuguese
	}

	public enum DisplayModeName : uint
	{
		Windowed,
		Fullscreen,
		Borderless_Fullscreen
	}
}
