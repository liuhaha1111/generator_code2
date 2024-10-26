package com.example.progect1.entity;

import com.baomidou.mybatisplus.annotation.TableName;
import com.baomidou.mybatisplus.annotation.IdType;
import java.time.LocalDateTime;
import com.baomidou.mybatisplus.annotation.TableId;
import java.io.Serializable;
import io.swagger.annotations.ApiModel;
import io.swagger.annotations.ApiModelProperty;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.experimental.Accessors;

/**
 * <p>
 * 主治医生信息表
 * </p>
 *
 * @author lyh
 * @since 2024-10-26
 */
@Data
@EqualsAndHashCode(callSuper = false)
@Accessors(chain = true)
@TableName("cpc_usr_info")
@ApiModel(value="CpcUsrInfo对象", description="主治医生信息表")
public class CpcUsrInfo implements Serializable {

    private static final long serialVersionUID = 1L;

    @ApiModelProperty(value = "cpc用户主键，雪花算法")
    @TableId(value = "cpc_usr_id", type = IdType.INPUT)
    private Long cpcUsrId;

    @ApiModelProperty(value = "主治医生姓名，数据库内加密")
    @TableField("cpc_usr_name")
    private String cpcUsrName;

    @ApiModelProperty(value = "主治医生手机号，数据库内加密")
    @TableField("cpc_usr_contact")
    private String cpcUsrContact;

    @ApiModelProperty(value = "主治医生工号，数据库内加密")
    @TableField("cpc_usr_work_id")
    private String cpcUsrWorkId;

    @ApiModelProperty(value = "主治医生简介，100字以内，数据库内加密")
    @TableField("cpc_usr_omitted")
    private String cpcUsrOmitted;

    @ApiModelProperty(value = "open_id")
    @TableField("cpc_usr_open_id")
    private String cpcUsrOpenId;

    @ApiModelProperty(value = "cpc单位主键，cpc管理中心主键")
    @TableField("cpc_usr_belonging")
    private Long cpcUsrBelonging;

    @ApiModelProperty(value = "cpc单位名称")
    @TableField("cpc_usr_belonging_name")
    private String cpcUsrBelongingName;

    @ApiModelProperty(value = "主治医师工作状态，0无法参与管理/1可以参与管理")
    @TableField("cpc_usr_status")
    private Integer cpcUsrStatus;

    @ApiModelProperty(value = "注册状态，0注册待审核/1注册成功")
    @TableField("register_status")
    private Integer registerStatus;


}
