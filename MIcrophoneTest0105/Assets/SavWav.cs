//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.IO;

//public static class SavWav : MonoBehaviour
//{
//    const int HEADER_SIZE = 44;
//    //RIFF, FMT, DATA������ ��� �������� 44����Ʈ�� ���´�.
//    //�׷��� ��� ����� 44�ε��ϴ�.

//    public static bool Save(string filename, AudioClip clip)
//    {
//        if (!filename.ToLower().EndsWith(".wav"))//�����̸��� .wav�� ������ �ʴ´ٸ�
//        {
//            filename += ".wav";
//        }
//        var filepath = Path.Combine(Application.persistentDataPath, filename);
//        // >Path.Combine = ���ڿ��� ��η� �����Ѵ�
//        // ex) Combine(String[]) - �ϳ��� ��η� ����
//        //Combine (String, String) - �ΰ��� ���ڿ��� �ϳ��� ��η� ����
//        //... (4������ ��밡���Ѱ����� ����)

//        //Application.persistentDataPath = �Ʒ��� ���� ��ΰ� �����ȴ�.
//        //On Win7 - C:/Users/Username/AppData/LocalLow/CompanyName/GameName
//        //On Android - / Data / Data / com.companyname.gamename / Files

//        Debug.Log(filepath);//���� ����� ���


//        //���� ���丮�� �����ϴ� ��� ���丮�� �����ϴ��� Ȯ��.
//        //�����Ѵٸ� ����
//        //�������� �ʴ´ٸ� ������ �����. maybe?
//        Directory.CreateDirectory(Path.GetDirectoryName(filepath));


//        /////////////using�� �� ����Ͽ�����////////////
//        //���������� using��
//        //IDisposable��ü�� �ùٸ� ����� �����ϴ� ���� ������ �������ִ°��� �ٷ� using���̴�
//        //File �� Font�� ���� Ŭ�������� �������� �ʴ� ���ҽ��� �׼��� �ϴ� ��ǥ���� Ŭ����
//        //�� ���� �ش� Ŭ�������� �� ����� �Ŀ��� ������ �ñ⿡ '����(Dispose)�Ͽ� �ش� ���ҽ�(�ڿ�)�� �ٽ� 
//        //'�ݳ�'�ؾ� �ϴ°�
//        //������, �Ź� �������� �ʴ� ���ҽ��� �׼����ϴ� Ŭ������ üũ�ؼ� Dispose�ϴ°��� �ð��� �Ǽ��� �߱��Ѵ�.
//        //�� �� using���� �̿��ϸ� �ش� ���ҽ� ������ ����� �� �� �ڵ����� Dispose�Ͽ� ������ �����ֱ⿡
//        //����� �ð��� ���� �Ǽ��� �����ش�.
//        using (var fileStream = CreateEmpty(filepath)) //���ο� ������ ����°�
//        {
//            ConverAndWrite(fileStream, clip);

//            WriteHeader(fileStream.clip);
//        }
//        return true; //TODO : ���� ���忡 �����ϸ� false�� ��ȯ
//        //<<FileStream>>
//        //FileStream = ���� �����
//        //�Ű�ü Ŭ������ ���� �����Ͽ� �а�, �� �� �ִ�.
//        //�Ϲ������� byte[]�迭�� �̿��� �б�/���⸦ ����

//        //ex) FileStream fs = new FileStream("���� �̸�", FileMode.OpenOrCreate)
//        //'������ �� �� ������ �����϶�' ��� ��
//        //�аų� ���~
//    }

//    public static AudioClip TrimSilence(AudioClip clip, float min)//ħ�� �ٵ��? 
//    {
//        var samples = new float[clip.samples];
//        //Ŭ���� ���� �����ͷ� �迭�� ä���.
//        //������ -1.0f ~ 1.0f������ �ε� �Ҽ���
//        //���ü��� float �迭�� ���̿� ���� ������.


//        //<<Clip.GetData>>
//        //offsetSamples�Ű� ������ ����Ͽ� Ŭ���� Ư�� ��ġ���� �б� ����
//        //�������� �б� ���̰� Ŭ�� ���̺��� ��� �бⰡ ���� �ѷ��ΰ� Ŭ���� ���ۺκп��� ������ ������ �д´�.
//        //����� ����� ������ ��� ����� �������⿡�� �ε������� �ҷ��������� ���� ������ ������ ��쿡��
//        //���õ����͸� �˻��� �� �ִ�.
//        //�׷��� ���� ��� �迭�� ���� ���� ���� ���� '0'���� ��ȯ
//        clip.GetData(samples, 0);
//        //Ŭ���� ������(���õ�����)�� 0������ �����´�.



//        /////////////�Ʒ� �Լ��� �Ű������� �����ϴµ� �������ϱ�//////////////////////////
//        return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
//    }
//    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz)
//    {
//        return TrimSilence(samples, min, channels, hz, false, false);
//        ///////////////�Ʒ��� �Լ� �Ű������� �����ε� �������� �𸣰ڴ�./////////////////////
//    }
//    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream)
//    {
//        int i; //������ ���밪�� min���� ���� ��� ���ߴµ� 
//        for (i = 0; i < samples.Count; i++)
//        {
//            if (Mathf.Abs(samples[i]) > min)
//            {
//                break;
//            }
//        }

//        samples.RemoveRange(0, i); //min���� ���� ������ ������Ų��

//        for(i = samples.Count -1; i> 0; i--)//����
//        {
//            if (Mathf.Abs(samples[i]) > min)
//            {
//                break;
//            }
//        }
//        //-------------------------------������---------------------------------
//    }

//    void Start()
//    {

//    }

//    void Update()
//    {

//    }
//}
