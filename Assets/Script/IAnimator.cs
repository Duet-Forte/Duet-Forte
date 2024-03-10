using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimator
{

    public void Dash();
    public void BackDash();
    public void Guard();

    public void Hurt(bool isSlash=true);

    public void Idle();




}
