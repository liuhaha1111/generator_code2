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
 * 审核员信息表
 * </p>
 *
 * @author lyh
 * @since 2024-10-26
 */
@Data
@EqualsAndHashCode(callSuper = false)
@Accessors(chain = true)
@TableName("auditor_info")
@ApiModel(value="AuditorInfo对象", description="审核员信息表")
public class AuditorInfo implements Serializable {

    private static final long serialVersionUID = 1L;

    @ApiModelProperty(value = "审核员主键，雪花算法")
    @TableId(value = "auditor_id", type = IdType.INPUT)
    private Long auditorId;

    @ApiModelProperty(value = "审核员姓名，数据库内加密")
    @TableField("auditor_name")
    private String auditorName;

    @ApiModelProperty(value = "审核员手机号，数据库内加密")
    @TableField("auditor_contact")
    private String auditorContact;

    @ApiModelProperty(value = "审核员工号，数据库内加密")
    @TableField("auditor_work_id")
    private String auditorWorkId;

    @ApiModelProperty(value = "open_id，可以为空")
    @TableField("auditor_open_id")
    private String auditorOpenId;

    @ApiModelProperty(value = "审核员单位主键，pci主键")
    @TableField("auditor_belonging")
    private Long auditorBelonging;

    @ApiModelProperty(value = "审核员单位名称")
    @TableField("auditor_belonging_name")
    private String auditorBelongingName;


}
