using System.Collections.Generic;
using UnityEngine;
using System;
public class BiologyAction
{
	internal Biology Biology;
	internal uFantasy.Enum.BiologyActions BiologyActions;

	public BiologyAction(Biology Parent)
	{
		Biology = Parent;
	}

	internal bool Attack()
	{
		//如果行動條依然在跑...
		if (Biology.ActionProgressbarValue < 1f) return false;

		//如果與目標的距離不在攻擊範圍內...
		if (IsTargetTooFar(3.5f)) { Biology.BiologyMovement.ActionMoveto(Biology.Target.transform.position); return false; }
		Biology.BiologyMovement.Stop();
		Biology.BiologyMovement.StartFaceTarget();

		//如果與目標不在我的正前方...
		if (Biology.BiologyMovement.GetIsFaceTarget() == false) { Biology.BiologyMovement.StartFaceTarget(); return false; }

		//完成攻擊
		Biology.Target.GetDamage(Biology.BiologyAttr.Atk);
		Biology.PlayAnimation(uFantasy.Enum.State.Attack_01);
		Biology.BiologyMovement.SetAvoidancePriority(100);

		//重置行動條

		//fixme:真正完成應該是要依照揮砍動畫是否播完
		return true;
	}

	private bool IsTargetTooFar(float dist)
	{
		float d = Vector3.Distance(Biology.Target.transform.position, Biology.transform.position);
		if (d <= dist) return false;
		return true;
	}

}