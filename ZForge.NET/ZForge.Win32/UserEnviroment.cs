using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Security.Principal;
using ZForge.Globalization;
using System.Windows.Forms;

namespace ZForge.Win32
{
	public static class UserEnviroment
	{
		public static bool IsVistaAbove()
		{
			return ((Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major >= 6));
		}

		public static bool IsWin2kAbove()
		{
			return ((Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major >= 5));
		}

		public static bool IsUACEnabled()
		{
			if (false == UserEnviroment.IsVistaAbove())
			{
				return false;
			}
			string sk = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
			RegistryKey rk = Registry.LocalMachine.OpenSubKey(sk);
			if (rk == null)
			{
				return false;
			}
			object o = rk.GetValue("EnableLUA");
			rk.Close();

			if (o == null)
			{
				return true;
			}
			return Convert.ToBoolean(o);
		}

		public static bool IsAdministrator()
		{
			WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			return wp.IsInRole(WindowsBuiltInRole.Administrator);
		}

		public static void ValidateUserEnviroment(string name, bool needAdmin)
		{
			if (false == UserEnviroment.IsWin2kAbove())
			{
				string m = string.Format(Translator.Instance.T("{0} ����������Windows 2000֮���Windows����ϵͳ��!"), name);
				throw new Exception(m);
			}
			if (needAdmin && false == UserEnviroment.IsAdministrator())
			{
				string m = string.Format(Translator.Instance.T("{0} ��Ҫ�����ڹ���ԱȨ����! ������ѡ��:"), name);
				m += "\n";
				m += Translator.Instance.T("1) ʹ�ù���Ա�˺����µ�¼ϵͳ.");
				m += "\n";
				m += Translator.Instance.T("2) ʹ������ķ�������Ӧ�ó��������Ȩ��:");
				if (UserEnviroment.IsVistaAbove())
				{
					m += "\n";
					m += string.Format(Translator.Instance.T("��Windows ��Դ��������ѡ��[{0}], �������Ҽ�, �ڵ����Ĳ˵���ѡ��[�Թ���Ա�������], ��������Ӧ�õ�����Ȩ��."), Application.ExecutablePath);
				}
				else
				{
					m += "\n";
					m += string.Format(Translator.Instance.T("��Windows ��Դ��������ѡ��[{0}], �������Ҽ�, �ڵ����Ĳ˵���ѡ��[���з�ʽ...], ��������Ӧ�õ�����Ȩ��."), Application.ExecutablePath);
				}
				throw new Exception(m);
			}
		}

	}
}
