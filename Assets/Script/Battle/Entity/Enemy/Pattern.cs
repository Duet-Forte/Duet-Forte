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
    private int[] signature;    // 적의 패턴을 박자로 기억해서 사용함.
                                // 예를 들어 4분의 1박자, 8분의 1박자 2개, 4분의 1박자로 이루어진 패턴은 4,8,8,4로 저장해서 사용함.
                                // 추후 데이터 테이블 단계에서 논의할 필요 있음. 솔직히 박자를 어떻게 다뤄야 할 지 감이 안옴.
    public int[] Signature { get { return signature; } set { signature = value; } }
}
