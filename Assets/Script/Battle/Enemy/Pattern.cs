using System;
using UnityEngine;

public struct Pattern
{
    public Pattern(int signatureData)
    {
        int signatureLength = (int)Math.Log10(signatureData) + 1;
        signature = new int[signatureLength];

        for(int index = signatureLength - 1; index >= 0; --index)
        {
            signature[index] = signatureData % 10;
            signatureData /= 10;
        }

    }
    private int[] signature;    // ���� ������ ���ڷ� ����ؼ� �����.
                                // ���� ��� 4���� 1����, 8���� 1���� 2��, 4���� 1���ڷ� �̷���� ������ 4,8,8,4�� �����ؼ� �����.
                                // ���� ������ ���̺� �ܰ迡�� ������ �ʿ� ����. ������ ���ڸ� ��� �ٷ�� �� �� ���� �ȿ�.
    public int[] Signature { get { return signature; } set { signature = value; } }
}
